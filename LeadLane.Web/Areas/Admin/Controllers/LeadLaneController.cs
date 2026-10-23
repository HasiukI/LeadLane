using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadLane.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class LeadLaneController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
