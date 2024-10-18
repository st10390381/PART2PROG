﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PART2PROG.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Hours Worked is required.")] 
        [Range(1, 100, ErrorMessage = "Hours Worked must be between 1 and 100.")] 
        public decimal HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required.")] 
        [Range(50, 1000, ErrorMessage = "Hourly Rate must be between 50 and 1000.")] 
        public decimal HourlyRate { get; set; }

        
        [Required]
        public decimal TotalAmount { get; set; }


        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")]
        public string Notes { get; set; }
        [Required]
        [CustomValidation(typeof(Claim), nameof(ValidateSubmissionDate))]
        public DateTime DateSubmitted { get; set; }
        // Status
        public string Status { get; set; } = "Pending";
        // Track approvals
        public bool IsApprovedByCoordinator { get; set; } = false;
        public bool IsApprovedByManager { get; set; } = false;

        [ForeignKey("ApplicationUser")] 
        public string ApplicationUserId { get; set; } 
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<SupportingDocument> Doc { get; set; }


        public static ValidationResult ValidateSubmissionDate(DateTime dateSubmitted, ValidationContext context)
        {
            var currentDate = DateTime.Now;
            if (dateSubmitted > currentDate)
            {
                return new ValidationResult("Date Submitted cannot be in the future.");
            }
            if (dateSubmitted.Month != currentDate.Month && dateSubmitted.Month != currentDate.AddMonths(-1).Month)
            {
                return new ValidationResult("Date Submitted can only be from the current month or previous month.");
            }
            return ValidationResult.Success;
        }
    }
}
