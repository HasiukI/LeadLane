using LeadLane.Domain.Leads.Models;
using LeadLane.Domain.Leads.Services;
using LeadLane.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadLane.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class LeadsController(LeadService leadService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? q, CancellationToken cancellationToken)
    {
        var leads = await leadService.SearchAsync(q, cancellationToken);
        var attention = string.IsNullOrWhiteSpace(q)
            ? await leadService.GetNeedingAttentionAsync(cancellationToken)
            : leads
                .Where(x =>
                    x.NextContactAt != null
                    && x.NextContactAt <= DateTimeOffset.UtcNow
                    && x.Status is not LeadStatus.Won and not LeadStatus.Lost)
                .OrderBy(x => x.NextContactAt)
                .ToList();

        var columns = Enum.GetValues<LeadStatus>()
            .ToDictionary(
                status => status,
                status => (IReadOnlyList<Lead>)leads
                    .Where(x => x.Status == status)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList());

        return View(new LeadBoardViewModel
        {
            Query = q,
            AttentionLeads = attention,
            Columns = columns
        });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await leadService.GetByIdAsync(id, cancellationToken);
        if (!result.IsSuccess || result.Data is null)
        {
            return NotFound();
        }

        return View(LeadDetailsViewModel.FromLead(result.Data));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(
        int id,
        LeadStatus status,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        var result = await leadService.UpdateStatusAsync(id, status, cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.ErrorMessage;
        }

        return LocalRedirectOrDetails(id, returnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateNotes(
        int id,
        string? notes,
        CancellationToken cancellationToken)
    {
        var result = await leadService.UpdateNotesAsync(id, notes, cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.ErrorMessage;
        }
        else
        {
            TempData["Success"] = "Notes saved.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateNextContact(
        int id,
        DateTime? nextContactAt,
        CancellationToken cancellationToken)
    {
        DateTimeOffset? value = nextContactAt.HasValue
            ? new DateTimeOffset(DateTime.SpecifyKind(nextContactAt.Value, DateTimeKind.Local)).ToUniversalTime()
            : null;

        var result = await leadService.UpdateNextContactAsync(id, value, cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.ErrorMessage;
        }
        else
        {
            TempData["Success"] = "Next contact updated.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    private IActionResult LocalRedirectOrDetails(int id, string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}
