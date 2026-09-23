using ContentParserApi.DTOs;
using ContentParserApi.Enums;
namespace ContentParserApi.Strategies
{
    public interface IParserStrategy
    {
        public CheckType Type { get; }
        public ResponseDto Parse(PayLoadDto payload);
    }
}
