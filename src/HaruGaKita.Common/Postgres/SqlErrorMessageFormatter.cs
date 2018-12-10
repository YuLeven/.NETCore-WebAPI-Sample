using System.Linq;
using System.Text.RegularExpressions;

namespace HaruGaKita.Common.Postgres
{
    public class SqlErrorMessageFormatter
    {
        private string _errorMessage;

        public SqlErrorMessageFormatter(string ErrorMessage)
        {
            _errorMessage = ErrorMessage;
        }

        public string Format()
        {
            var keyAndValueRegex = new Regex(@"\(([^()]+)\)");
            var stripSymbolsRegex = new Regex("[\\(|\\)|\\\"]");

            string[] matches = keyAndValueRegex.Matches(_errorMessage)
                            .Cast<Match>()
                            .Select(m => stripSymbolsRegex.Replace(m.Value, ""))
                            .ToArray();

            return $"{matches[0]} {matches[1]} already exists.";
        }
    }
}