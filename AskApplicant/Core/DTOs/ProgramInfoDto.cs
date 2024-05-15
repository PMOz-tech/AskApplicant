namespace AskApplicant.Core.DTOs
{
    public class ProgramInfoDto
    {
         public string Title { get; set; }
         public string Description { get; set; }
        public bool IsPhoneNumberRequired { get; set; }
        public bool IsNationalityRequired { get; set; }
        public bool IsResidenceRequired { get; set; }
        public bool IsIdentificationNumberRequired { get; set; }
        public bool IsDobRequired { get; set; }
        public bool IsGenderRequired { get; set; }
        public bool IsEmailRequired { get; set; } = true;
        public bool IsFirstNameRequired { get; set; } = true;
        public bool IsLastNameRequired { get; set; } = true;
    }
}
