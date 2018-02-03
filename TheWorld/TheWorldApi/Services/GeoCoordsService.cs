using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Services;

namespace TheWorldApi.Services
{
    public class GeoCoordsService
    {
        public readonly ILogger<GeoCoordsService> Logger;

        public readonly IConfiguration Configuration;

        public GeoCoordsService(ILogger<GeoCoordsService> logger, IConfiguration configuration)
        {
            this.Logger = logger;
            this.Configuration = configuration;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCoordsResult()
            {
                Success = false,
                Message = "Failed to get coordinates"
            };

            string sites = "10.*;172.*;'\'*.intraxa;'\'*.corp.intraxa;helpdesk.axa.gr;http://evserver1.axa.gr;pbxreport.axa.gr;ath*.axa.gr;supportcenter.axa.gr;one.axa.com;'\'*.one.axa.com;smlogon.axa.com;pp-smlogon.axa.com;dev-smlogon.axa.com;bcm.axa.gr;keys.axa.gr;hr.axa.gr;customerdesk.axa.gr;hr-desk.axa.gr;ddx-uat.axa.gr;'\'*ddx-prd.axa.gr;e-hr.axa.gr;http://loopback;http://inamreip01.ppmail.ppservices.axa-tech.intraxa;http://dev.virtualearth.net;http://dev.virtualearth.net/REST;http://dev.virtualearth.net/REST/v1;http://dev.virtualearth.net/REST/v1/Locations?q=Aigion&key=AmRCXd90Jru_lOphgvrEPlzNcBm_C_LCvBvEqqLjGrff8MZUT3u_VZAaTQaqQGSW"; 
            string[] sitesArr = sites.Split(';');

            var apiKey = "AmRCXd90Jru_lOphgvrEPlzNcBm_C_LCvBvEqqLjGrff8MZUT3u_VZAaTQaqQGSW";
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={apiKey}";

            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://athprxsrv01:8080", true, sitesArr),
                UseProxy = true,
                Credentials = CredentialCache.DefaultNetworkCredentials //new NetworkCredential("datacom_psagianos", "welcome1!", "axa")     
            };

            var client = new HttpClient();//handler
            //var byteArray = Encoding.ASCII.GetBytes("datacom_psagianos:welcome1!:axa");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var json = await client.GetAsync(url).ConfigureAwait(false);

            if (json.IsSuccessStatusCode)
            {
                // Read out the results
                // Fragile, might need to change if the Bing API changes
                var results = JObject.Parse(json.ToString());
                var resources = results["resourceSets"][0]["resources"];
                if (!results["resourceSets"][0]["resources"].HasValues)
                {
                    result.Message = $"Could not find '{name}' as a location";
                }
                else
                {
                    var confidence = (string)resources[0]["confidence"];
                    if (confidence != "High")
                    {
                        result.Message = $"Could not find a confident match for '{name}' as a location";
                    }
                    else
                    {
                        var coords = resources[0]["geocodePoints"][0]["coordinates"];
                        result.Latitude = (double)coords[0];
                        result.Longitude = (double)coords[1];
                        result.Success = true;
                        result.Message = "Success";
                    }
                }
            }
            
            return result;
        }
    }
}
