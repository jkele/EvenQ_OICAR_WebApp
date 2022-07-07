using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Eveq_Oicar_web
{
    public class GlobalVariable
    {
        public static HttpClient WebApiClient = new HttpClient();
        static GlobalVariable()
        {
            WebApiClient.BaseAddress = new Uri("https://evenq.azurewebsites.net/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
