using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Data;

namespace PART2PROG.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDBContext _context;  // Database context
        private readonly UserManager<IdentityUser> _userManager;  // Manages user information

        // Constructor initializes the context and user manager
        public LecturerController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Displays the dashboard for a lecturer, allowing filtering by date
        public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
        {
            // Get the current user and their ID
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            // Query to fetch claims related to the current lecturer
            var claimsQuery = _context.Claims
                .Include(c => c.Doc)  // Include related document information
                .Where(c => c.ApplicationUserId == userId);  // Filter claims by user

            // Filter claims by the provided date range, if available
            if (startDate.HasValue && endDate.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.DateSubmitted >= startDate.Value && c.DateSubmitted <= endDate.Value);
            }

            // Execute the query and return the results to the view
            var claims = await claimsQuery.ToListAsync();
            return View(claims);
        }
    }
}

//Reference List:

//https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0
//-Microsoft
//-Accessed 17 October 2024

//Reference list:
//https://www.w3schools.com/html/
//-w3schools
//- Accessed 15 October 2024

