using log4net.ObjectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using log4net.Util;
using MedicalWebService.Models;

namespace MedicalWebService.Renderers
{
    public class PatientRender : IObjectRenderer
    {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            if (obj == null)
            {
                writer.Write(SystemInfo.NullText);
            }
            PatientLogObject log = obj as PatientLogObject;
            if (log != null)
            {
                if (log.Execute == Enums.ExecuteEnumType.ENTERING)
                {
                        writer.Write("Entering {0} with parametsrs [ {1} ]...",
                                       log.Method,
                                      log.Parameter
                                       );
                }
                else {
                        writer.Write("Exiting {0} with return value [ {1} ]...",
                                       log.Method,
                                      log.Parameter
                                       );
                }

            }
            else
            {
                writer.Write(SystemInfo.NullText);
            }
           
        }
    }
}