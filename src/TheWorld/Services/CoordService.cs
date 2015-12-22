using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Update.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace TheWorld.Services
{
    public class CoordService
    {
        private ILogger<CoordService> _logger;

        public CoordService(ILogger<CoordService> logger)
        {
            _logger = logger;
        }

        public async Task<CoordServiceResult> Lookup(string location)
        {
            var result = new CoordServiceResult()
            {
                Success = false,
                Message = "Undetermined failure while looking up coordinates"
            };

            // Lookup Coordinates
            var secretKey = Startup.Configuration["AppSettings:GoogleApiKey"];
            var encodedName = WebUtility.UrlEncode(location);
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedName}&key={secretKey}";

            var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var coordinates = results["results"][0]["geometry"]["location"];

            if (coordinates == null)
            {
                result.Message = "The location could not be geocoded.";
                return result;
            }

            result.Latitude = (double)coordinates["lat"];
            result.Longitude = (double)coordinates["lng"];
            result.Success = true;
            result.Message = "Success";

            return result;
        }
    }
}
