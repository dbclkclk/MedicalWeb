using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalWebService.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
        
    }
}