using MedicalWebService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalWebService.Models
{
    public class PatientLogObject
    {
        public String Method { get; set; }

        public object Parameter { get; set; }

        public ExecuteEnumType Execute { get; set; }
    }
}