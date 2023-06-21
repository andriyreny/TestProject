using Newtonsoft.Json.Linq;

namespace TestProject.Helpers
{
    public class CountryNameComparer : IComparer<JObject>
    {
        private string _fieldCountryName;
        private string _sortDirection;

        public CountryNameComparer(string fieldCountryName, string sortDirection)
        {
            _fieldCountryName = fieldCountryName;
            _sortDirection = sortDirection;
        }

        public int Compare(JObject? a, JObject? b)
        {
            var compareResult = string.Compare((string?)a?.SelectToken(_fieldCountryName), (string?)b?.SelectToken(_fieldCountryName));

            if (_sortDirection == "descend")
            {
                compareResult = compareResult * -1;
            }

            return compareResult;
        }
    }
}
