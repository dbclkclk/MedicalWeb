using MedicalWebService.Enums;
using MedicalWebService.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalWebService.Models
{
    public class Patient
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="First Name is a required field")]
        [MaxLength(64, ErrorMessage ="First name should be 64 Characters")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage ="First name should not contain special characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [MaxLength(64, ErrorMessage = "First name should be 64 Characters")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "First name should not contain special characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [EnumDataType(typeof(GenderTypes),ErrorMessage ="Incorrect Gender Type")]
        public GenderTypes Gender { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [MinimumDays(ErrorMessage ="Appointment day cannot be more than 7 days in the past")]
        public DateTime Appointment { get; set; }

        [JsonIgnore]
        public int? DoctorId { get; set; }

        [JsonIgnore]
        public ApplicationUser Doctor { get; set; }

        public int? NurseId { get; set; }

        public ApplicationUser Nurse { get; set; }

        [MaxLength(256, ErrorMessage ="The max length for comment is 256 characters")]
        public string DoctorComment { get; set; }

        [DefaultValue(false)]
        public bool completed { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}