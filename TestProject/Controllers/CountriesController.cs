using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using TestProject.Helpers;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private string fieldCountryName = "name.common";
        private string fieldCountryPopulation = "population";
        private const string propertyAscend = "ascend";
        private const string propertyDescend = "descend";
        private const string apiUrl = "https://restcountries.com/v3.1/all";

        [HttpGet(Name = "GetCountries")]
        public async Task<string> Get(string? countryName = null, int countryPopulation = 0, string? sortOption = null)
        {
            var httpClient = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            string jsonContent = await response.Content.ReadAsStringAsync();
            JArray countriesArray = JArray.Parse(jsonContent);

            if (!string.IsNullOrEmpty(countryName))
            {
                var jArray = countriesArray.Where(d => (d.SelectToken(fieldCountryName)?.ToString() ?? string.Empty).Contains(countryName, StringComparison.OrdinalIgnoreCase));
                countriesArray = new JArray(jArray);
            }

            if (countryPopulation > 0)
            {
                var jArray = countriesArray.Where(d => (d.SelectToken(fieldCountryPopulation)?.ToObject<long>() / 1000000) < countryPopulation);
                countriesArray = new JArray(jArray);
            }

            if (!string.IsNullOrEmpty(sortOption))
            {
                if (sortOption.ToLower() == propertyAscend)
                {
                    var comparer = new CountryNameComparer(fieldCountryName, propertyAscend);
                    countriesArray = new JArray(countriesArray.AsQueryable().OrderBy(obj => (JObject)obj, comparer));
                }
                if (sortOption.ToLower() == propertyDescend)
                {
                    var comparer = new CountryNameComparer(fieldCountryName, propertyDescend);
                    countriesArray = new JArray(countriesArray.AsQueryable().OrderBy(obj => (JObject)obj, comparer));
                }
            }

            return countriesArray.ToString();
        }
    }
}