using MedicalFrontend.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFrontend.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public GenderTypes Gender { get; set; }

        public DateTime Appointment { get; set; }

        public string DoctorComment { get; set; }

        public bool completed { get; set; }

        public byte[] RowVersion { get; set; }
    }
}