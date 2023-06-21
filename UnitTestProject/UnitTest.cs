using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net;
using TestProject.Configurations;
using TestProject.Controllers;

namespace UnitTestProject
{
    public class UnitTests
    {
        private const string FieldCountryName = "name.common";
        private const string FieldCountryPopulation = "population";

        private Mock<IOptions<CustomSettings>> m_sustomSettingsOptions;
        private Mock<HttpMessageHandler> m_handlerMock;
        private CountriesController m_countriesController;
        private HttpClient m_httpClient;
        private string? m_countryName = null;
        private int m_countryPopulation = 0;
        private string? m_sortOption = null;
        private int m_paginateNumber = 0;

        [SetUp]
        public void Setup()
        {
            string filePath = Directory.GetCurrentDirectory() + "\\JsTestData.json";
            string jsText = File.ReadAllText(filePath);

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsText),
            };

            m_handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            m_handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            m_httpClient = new HttpClient(m_handlerMock.Object);

            m_sustomSettingsOptions = new Mock<IOptions<CustomSettings>>();
           
            m_sustomSettingsOptions.Setup(x => x.Value)
                .Returns(new CustomSettings
                {
                    ApiUrl = "http://test"
                });

            m_countriesController = new CountriesController(m_httpClient, m_sustomSettingsOptions.Object);
        }

        [Test]
        public async Task FilterCountriesByName_ReturnData()
        {
            m_countryName = "fa";
            m_countryPopulation = 0;
            m_paginateNumber = 0;
            m_sortOption = null;

            var jsText = await m_countriesController.Get(m_countryName, m_countryPopulation, m_sortOption, m_paginateNumber);
            var jArray = JArray.Parse(jsText);
            var result = jArray.FirstOrDefault()?.SelectToken(FieldCountryName)?.ToString() ?? string.Empty;

            Assert.That(result, Is.EqualTo("fa_countryName"));
        }

        [Test]
        public async Task FilterCountriesByPopulation_ReturnData()
        {
            m_countryPopulation = 2;
            m_countryName = null;
            m_paginateNumber = 0;
            m_sortOption = null;

            var jsText = await m_countriesController.Get(m_countryName, m_countryPopulation, m_sortOption, m_paginateNumber);
            var jArray = JArray.Parse(jsText);
            var result = jArray.FirstOrDefault()?.SelectToken(FieldCountryPopulation)?.ToString() ?? string.Empty;

            Assert.That(result, Is.EqualTo("1000000"));
        }

        [Test]
        public async Task SortCountries_Descend_ReturnData()
        {
            m_countryName = null;
            m_countryPopulation = 0;
            m_paginateNumber = 0;
            m_sortOption = "descend";

            var jsText = await m_countriesController.Get(m_countryName, m_countryPopulation, m_sortOption, m_paginateNumber);
            var jArray = JArray.Parse(jsText);
            var result = jArray.FirstOrDefault()?.SelectToken(FieldCountryName)?.ToString() ?? string.Empty;

            Assert.That(result, Is.EqualTo("fc_countryName"));
        }

        [Test]
        public async Task SortCountries_Ascend_ReturnData()
        {
            m_countryName = null;
            m_countryPopulation = 0;
            m_paginateNumber = 0;
            m_sortOption = "ascend";

            var jsText = await m_countriesController.Get(m_countryName, m_countryPopulation, m_sortOption, m_paginateNumber);
            var jArray = JArray.Parse(jsText);
            var result = jArray.FirstOrDefault()?.SelectToken(FieldCountryName)?.ToString() ?? string.Empty;

            Assert.That(result, Is.EqualTo("fa_countryName"));
        }

        [Test]
        public async Task PaginateCountries_ReturnData()
        {
            m_countryPopulation = 0;
            m_countryName = null;
            m_sortOption = null;
            m_paginateNumber = 1;

            var jsText = await m_countriesController.Get(m_countryName, m_countryPopulation, m_sortOption, m_paginateNumber);
            var jArray = JArray.Parse(jsText);
            var result = jArray.Count;

            Assert.That(result, Is.EqualTo(1));
        }
    }
}