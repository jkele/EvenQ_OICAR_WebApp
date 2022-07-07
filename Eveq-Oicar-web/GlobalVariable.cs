﻿using System;
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

        private static int _counter = 22;
        private static readonly object _lockObject = new object();
        static GlobalVariable()
        {
            WebApiClient.BaseAddress = new Uri("https://evenq.azurewebsites.net/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void Increment()
        {
            lock (_lockObject)
            {
                _counter++;
            }
        }
        public static int Counter
        {
            get
            {
                lock (_lockObject)
                {
                    return _counter;
                }
            }
        }
    }
}
