﻿namespace Perfx.SamplePlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Perfx;

    public class MyPlugin2 : IPlugin
    {
        public Task<string> GetAuthToken(Settings settings)
        {
            //  NOTE: By default Perfx uses IPublicClientApplication.AcquireTokenSilent
            //        If you want to override that behavior and provide a custom implementation, go ahead...
            var userId = settings.UserId;
            var pwd = settings.Password;

            // Get more settings as required...

            return Task.FromResult("someToken2");
        }

        public Task<List<Endpoint>> GetEndpointDetails(Settings settings)
        {
            //  NOTE: By default Perfx uses Documents/Perfx/Perfx_Inputs.xlsx
            //        If you want to override that behavior and provide a custom implementation, go ahead...
            var endpointDetails = new List<Endpoint>();
            foreach (var endpoint in settings.Endpoints.Select((e, i) => (url: e, index: i)))
            {
                if (endpoint.url.Contains("odata"))
                {
                    endpointDetails.Add(new Endpoint { Method = HttpMethod.Get.ToString(), Query = "?$top=10" }); // Do whatever - based on the endpoint
                }
                else if (endpoint.url.EndsWith("route2"))
                {
                    endpointDetails.Add(new Endpoint { Method = HttpMethod.Get.ToString(), Query = "/1" }); // Do whatever - based on the endpoint
                }
            }

            return Task.FromResult(endpointDetails);
        }

        ////public Task<dynamic> ProcessRequest(dynamic record, CancellationToken stopToken = default)
        ////{
        ////    // Get the response whichever way you like from the record details ('record' is of type 'Perfx.Result') and...
        ////    // SET:
        ////    //  record.local_ms // e.g. taskWatch.ElapsedMilliseconds;
        ////    //  record.result // e.g. $"{(int)response.StatusCode}: {response.ReasonPhrase}";
        ////    //  record.size_b // e.g. response.Content.Headers.ContentLength;

        ////    throw new NotImplementedException();
        ////}
    }
}