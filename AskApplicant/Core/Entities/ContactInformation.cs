using AskApplicant.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AskApplicant.Core.Entities
{
    public class ContactInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public string? Residence { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? Dob { get; set; }
        public string? Gender { get; set; }
        public ObjectId ProgramInfoId { get; set; }

    }
}
