using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PART2PROG.Models
{
    public class SupportingDocument
    {
        public int SupportingDocumentId { get; set; } 

        [Required(ErrorMessage = "Document Name is required.")] 
        [MaxLength(255)] 
        public string DocumentName { get; set; } 

        [Required] 
        public DateTime UploadDate { get; set; }
        
        [Required(ErrorMessage = "File Path is required.")] 
        public string FilePath { get; set; } 

        [ForeignKey("Claim")] 
        public int ClaimId { get; set; } 
        public virtual Claim Claim { get; set; }
    }
}
