using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        public CountriesController()
        {
        }

        [HttpGet(Name = "GetCountries")]
        public async Task<string> Get(string argument1, int argument2, string argument3)
        {
            var httpClient = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://restcountries.com/v3.1/all");
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            string jsonContent = await response.Content.ReadAsStringAsync();
            JArray countriesArray = JArray.Parse(jsonContent);

            return countriesArray.ToString();
        }
    }
}