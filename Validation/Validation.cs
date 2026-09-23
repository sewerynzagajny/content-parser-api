using ContentParserApi.DTOs;
using ContentParserApi.Enums;

namespace ContentParserApi.Validation
{
    public class Validation
    {
        public PayLoadDto Validate(PayLoadDto payload)
        {
            
            if (!Enum.IsDefined(payload.Type))
            {
                throw new ArgumentOutOfRangeException($"Nieprawidłowy typ: {payload.Type}");
            }

            return payload;
        }
    }
}
