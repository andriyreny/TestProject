using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TestProject.Helpers;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly HttpClient m_httpClient;
        private string m_fieldCountryName = "name.common";
        private string m_fieldCountryPopulation = "population";

        private const string FieldAscend = "ascend";
        private const string FieldDescend = "descend";
        private const string ApiUrl = "https://restcountries.com/v3.1/all";

        public CountriesController(HttpClient httpClient)
        {
            m_httpClient = httpClient;
        }

        [HttpGet(Name = "GetCountries")]
        public async Task<string> Get(string? countryName = null, 
            int countryPopulation = 0, 
            string? sortOption = null, 
            int paginateNumber = 0)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
            using HttpResponseMessage response = await m_httpClient.SendAsync(request);
            string jsonContent = await response.Content.ReadAsStringAsync();
            JArray countriesArray = JArray.Parse(jsonContent);

            countriesArray = FilterCountriesByName(countriesArray, countryName);
            countriesArray = FilterCountriesByPopulation(countriesArray, countryPopulation);
            countriesArray = SortCountries(countriesArray, sortOption);
            countriesArray = PaginateCountries(countriesArray, paginateNumber);

            return countriesArray.ToString();
        }

        private JArray FilterCountriesByName(JArray countriesArray, string? countryName = null)
        {
            if (!string.IsNullOrEmpty(countryName))
            {
                var jArray = countriesArray.Where(d =>
                (d.SelectToken(m_fieldCountryName)?.ToString() ?? string.Empty).Contains(countryName, StringComparison.OrdinalIgnoreCase));
                countriesArray = new JArray(jArray);
            }

            return countriesArray;
        }

        private JArray FilterCountriesByPopulation(JArray countriesArray, int countryPopulation = 0)
        {
            if (countryPopulation > 0)
            {
                var jArray = countriesArray.Where(d =>
                (d.SelectToken(m_fieldCountryPopulation)?.ToObject<long>() / 1000000) < countryPopulation);
                countriesArray = new JArray(jArray);
            }

            return countriesArray;
        }

        private JArray SortCountries(JArray countriesArray, string? sortOption = null)
        {
            if (!string.IsNullOrEmpty(sortOption))
            {
                if (sortOption.ToLower() == FieldAscend)
                {
                    var comparer = new CountryNameComparer(m_fieldCountryName, FieldAscend);
                    countriesArray = new JArray(countriesArray.AsQueryable().OrderBy(obj => (JObject)obj, comparer));
                }
                if (sortOption.ToLower() == FieldDescend)
                {
                    var comparer = new CountryNameComparer(m_fieldCountryName, FieldDescend);
                    countriesArray = new JArray(countriesArray.AsQueryable().OrderBy(obj => (JObject)obj, comparer));
                }
            }

            return countriesArray;
        }

        private JArray PaginateCountries(JArray countriesArray, int paginateNumber = 0)
        {
            if (paginateNumber > 0)
            {
                countriesArray = new JArray(countriesArray.Take(paginateNumber));
            }

            return countriesArray;
        }
    }
}