using ContentParserApi.DTOs;
using System.Text;

namespace ContentParserApi.Services
{
    public class DecodeBase64Service
    {
        public PayLoadDto DecodeBase64(PayLoadDto payLoad)
        {
            if (string.IsNullOrWhiteSpace(payLoad.Content) || payLoad.Content.Length % 4 != 0)
            {
                throw new FormatException("Invalid base64 string");
            }

            Span<byte> buffer = stackalloc byte[payLoad.Content.Length];
            if (Convert.TryFromBase64String(payLoad.Content, buffer, out int bytesWritten))
            {
                string decodeContentStr = Encoding.UTF8.GetString(buffer.Slice(0, bytesWritten));
                PayLoadDto decodePayLoad = new PayLoadDto
                {
                    Type = payLoad.Type,
                    Content = decodeContentStr,
                };

                return decodePayLoad;
            }
            else
            {
                throw new FormatException("Invalid base64 string");
            }
        }
    }
}
