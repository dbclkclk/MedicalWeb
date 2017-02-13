using MedicalWebService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalWebService.Models
{
    public class ApplicationUserLogObject
    {
        public ApplicationUser ApplicationUser { get; set; }
        public ExecuteEnumType ExecuteType { get; set; }
        public string Method { get; set; }
    }
}