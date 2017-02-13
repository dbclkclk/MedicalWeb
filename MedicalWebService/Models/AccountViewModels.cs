using MedicalWebService.Enums;
using MedicalWebService.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalWebService.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Id { get; set; }

        public string Role { get; set; }


        public string UserName { get; set; }

        public string Email { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }


    public class PatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="First Name is a required field")]
        [MaxLength(64, ErrorMessage ="First name should be 64 Characters")]
        [RegularExpression("",ErrorMessage ="First name should not contain special characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [MaxLength(64, ErrorMessage = "First name should be 64 Characters")]
        [RegularExpression("^[a-zA-Z0-9]{4,10}$", ErrorMessage = "First name should not contain special characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [EnumDataType(typeof(GenderTypes),ErrorMessage ="Incorrect Gender Type")]
        public GenderTypes Gender { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [MinimumDays(ErrorMessage ="Appointment day cannot be more than 7 days in the past")]
        public DateTime Appointment { get; set; }
        
        [MaxLength(256, ErrorMessage ="The max length for comment is 256 characters")]
        public string DoctorComment { get; set; }

        public byte[] RowVersion { get; set; }

        public bool completed { get; set; }
    }
}
