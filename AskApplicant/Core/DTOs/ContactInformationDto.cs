using AskApplicant.Core.Enums;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace AskApplicant.Core.DTOs
{
    public class ContactInformationDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public string? Residence { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? Dob { get; set; }
        public Gender? Gender { get; set; }
    }
    
 
}
