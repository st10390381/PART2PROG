namespace PART2PROG.Models
{
    //HR model Created
    public class HRViewModel
    {
        public int ClaimId { get; set; }
        public string LecturerName { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }
        public bool IsApprovedByCoordinator { get; set; }
        public bool IsApprovedByManager { get; set; }
    }
}
