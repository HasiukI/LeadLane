using System.Diagnostics;
using LeadLane.Domain.Leads.Services;
using LeadLane.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadLane.Web.Controllers;

public class HomeController(LeadService leadService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new CreateLeadViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CreateLeadViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await leadService.CreateAsync(
            model.Name,
            model.Phone,
            model.Email,
            model.Message);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Could not submit the form");
            return View(model);
        }

        TempData["Success"] = "Thank you. We will contact you soon.";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
