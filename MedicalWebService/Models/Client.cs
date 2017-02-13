using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalWebService.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string ClientSecret { get; set; }

        public string ClientRedirectURL { get; set; }
    }
}