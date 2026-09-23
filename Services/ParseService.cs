using ContentParserApi.Strategies;
using ContentParserApi.DTOs;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.Json;

namespace ContentParserApi.Services
{
    public class ParseService
    {
        private readonly IEnumerable<IParserStrategy> _strategies;

        public ParseService(IEnumerable<IParserStrategy> strategy)
        {
            _strategies = strategy;
        }

        public ResponseDto GetParse(PayLoadDto decodedPayLoad)
        {
            var strategy = _strategies.FirstOrDefault(el => el.Type == decodedPayLoad.Type);
            if (strategy == null)
            {
                throw new ArgumentOutOfRangeException(
                        nameof(decodedPayLoad),
                        $"Brak strategii prasowania dla typu {decodedPayLoad.Type}");
            }

            return strategy.Parse(decodedPayLoad);
        }
    }
}
