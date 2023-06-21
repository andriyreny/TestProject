using Newtonsoft.Json.Linq;

namespace TestProject.Comparers
{
    public class CountryNameComparer : IComparer<JObject>
    {
        private readonly string _fieldCountryName;
        private readonly string _sortDirection;

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
