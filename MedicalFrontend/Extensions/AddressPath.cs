using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MedicalFrontend.Extensions
{
	public static class AddressPath
	{
        public static string UserInfo
        {
            get {
                return System.Configuration.ConfigurationManager.AppSettings["apibase"]+System.Configuration.ConfigurationManager.AppSettings["oauthserverUserInfo"];
            }
        }
        public static string ServerToken {
            get {
                return System.Configuration.ConfigurationManager.AppSettings["apibase"] +System.Configuration.ConfigurationManager.AppSettings["oauthserverlogin"];
            }
        }
        public static string Patients
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["apibase"] +System.Configuration.ConfigurationManager.AppSettings["oauthserverPatients"];
            }
        }
    }
}