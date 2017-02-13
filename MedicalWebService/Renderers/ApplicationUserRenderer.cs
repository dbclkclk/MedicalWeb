using log4net.ObjectRenderer;
using log4net.Util;
using MedicalWebService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MedicalWebService.Renderers
{
    public class ApplicationUserRenderer : IObjectRenderer
    {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            if (obj == null)
            {
                writer.Write(SystemInfo.NullText);
            }
            ApplicationUserLogObject  log = obj as ApplicationUserLogObject;
            if (log != null)
            {
                if (log.ExecuteType == Enums.ExecuteEnumType.ENTERING)
                {
                        writer.Write("Entering {0} with parameters [ Id={1} User Name={2}]...",
                                        log.Method,
                                        log.ApplicationUser.Id,
                                        log.ApplicationUser.UserName
                                       );
                }
                else
                {

                        writer.Write("Entering {0} with parameters [ Id={1} User Name={2} ]...",
                                        log.Method,
                                        log.ApplicationUser.Id,
                                        log.ApplicationUser.UserName);
                   
                }

            }
            else
            {
                writer.Write(SystemInfo.NullText);
            }

        }
    }
}