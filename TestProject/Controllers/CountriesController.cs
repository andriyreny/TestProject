using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private const string propertyName = "name";
        private const string propertyCommon = "common";
        private const string apiUrl = "https://restcountries.com/v3.1/all";

        [HttpGet(Name = "GetCountries")]
        public async Task<string> Get(string? countryName = null, int argument2 = 0, string? argument3 = null)
        {
            var httpClient = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            string jsonContent = await response.Content.ReadAsStringAsync();
            JArray countriesArray = JArray.Parse(jsonContent);

            if (!string.IsNullOrEmpty(countryName))
            {
                var jArray = countriesArray.Where(d => d[propertyName] != null 
                && d[propertyName]?[propertyCommon] != null
                && ((string?)d[propertyName]?[propertyCommon] ?? string.Empty).Contains(countryName, StringComparison.OrdinalIgnoreCase));

                countriesArray = new JArray(jArray);
            }

            return countriesArray.ToString();
        }
    }
}