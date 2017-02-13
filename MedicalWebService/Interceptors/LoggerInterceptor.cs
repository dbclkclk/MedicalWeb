using Castle.DynamicProxy;
using log4net;
using MedicalWebService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MedicalWebService.Interceptors
{
    public class LoggerInterceptor : IInterceptor
    {
      
        public ILog logger { get; set; }
        public LoggerInterceptor()
        {
 
        }

        public void Intercept(IInvocation invocation)
        {
                this.logger.Debug(new PatientLogObject {
                    Execute = Enums.ExecuteEnumType.ENTERING,
                    Method = invocation.Method.Name,
                    Parameter = JsonConvert.SerializeObject(invocation.Arguments, Formatting.None,new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                } );
                invocation.Proceed();

                this.InterceptAsync((dynamic)invocation.ReturnValue, invocation);

        }

        private async Task InterceptAsync(Task task, IInvocation invocation)
        {
            await task.ConfigureAwait(false);
            this.logger.Debug(new PatientLogObject
            {
                Execute = Enums.ExecuteEnumType.EXITING,
                Method = invocation.Method.Name,
                Parameter = "void"
            });
        }


        private async Task InterceptAsync<T>(Task<T> task, IInvocation invocation)
        {
            var result = await task.ConfigureAwait(false);

            this.logger.Debug(new PatientLogObject
            {
                Execute = Enums.ExecuteEnumType.EXITING,
                Method = invocation.Method.Name,
                Parameter = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
           });
        }
    }
}