using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PART2PROG.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
