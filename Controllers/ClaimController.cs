using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Data;
using PART2PROG.Models;

namespace PART2PROG.Controllers
{
    [Authorize (Roles = "Lecturer")]
    public class ClaimController : Controller
    {
        private readonly ApplicationDBContext _context; //interact with the database.
        private readonly UserManager<IdentityUser> _userManager; //helps manage user accounts, like retrieving the currently logged-in user.
        private readonly IWebHostEnvironment _environment;

        public ClaimController(ApplicationDBContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }
       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SupportingDocuments == null || model.SupportingDocuments.Count == 0)
            {
                ModelState.AddModelError("", "At least one supporting document must be attached.");
                return View(model);
            }

            bool isInvalidFile = false; 
            foreach (var file in model.SupportingDocuments)
            {
                if (file.ContentType != "application/pdf" || file.Length > 15 * 1024 * 1024)
                {
                    ViewBag.InvalidFile = true; 
                    isInvalidFile = true;
                    ModelState.AddModelError("", "Only PDF files under 15 MB are allowed.");
                    return View(model);
                }
            }

             if (!isInvalidFile)
            {
                var user = await _userManager.GetUserAsync(User);

                var claim = new Claim
                {
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    Notes = model.Notes,
                    DateSubmitted = DateTime.Now,
                    ApplicationUserId = user.Id,
                    TotalAmount = model.HourlyRate * model.HoursWorked

                };
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                foreach (var file in model.SupportingDocuments)
                { var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                     var doc = new SupportingDocument
                    {
                        ClaimId = claim.ClaimId,
                        DocumentName = uniqueFileName,
                        FilePath = filePath
                    };

                    _context.Doc.Add(doc);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                 return RedirectToAction("Dashboard", "Lecturer");
            }
            return View(model);
        }

        // GET: Claims/Track
        public async Task<IActionResult> TrackClaims()
        {
            var currentUser = await _userManager.GetUserAsync(User);

             var claims = _context.Claims
                .Where(c => c.ApplicationUserId == currentUser.Id)
                .ToList();
             return View(claims);
        }


        

        // GET: Claims/Edit/5
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var claim = await _context.Claims.FindAsync(id);
        //    if (claim == null)
        //    {
        //        return NotFound();
        //    }

        //    // Map Claim to ClaimViewModel
        //    var model = new ClaimViewModel
        //    {
        //        ClaimId = claim.ClaimId,
        //        HoursWorked = claim.HoursWorked,
        //        HourlyRate = claim.HourlyRate,
        //        Notes = claim.Notes,
        //        // Add other necessary fields from Claim to ClaimViewModel
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, ClaimViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var claim = await _context.Claims.FindAsync(id);
        //    if (claim == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update claim with model values
        //    claim.HoursWorked = model.HoursWorked;
        //    claim.HourlyRate = model.HourlyRate;
        //    claim.Notes = model.Notes;
        //    // Update other fields as necessary

        //    await _context.SaveChangesAsync();

        //    TempData["SuccessMessage"] = "Claim updated successfully!";
        //    return RedirectToAction(nameof(TrackClaims));
        //}

        
    }
}

