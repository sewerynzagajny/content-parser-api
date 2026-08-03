using ContentParserApi.DTOs;
using ContentParserApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContentParserApi.Controllers
{
    [Route("api/v1/parse-content")]
    [ApiController]
    public class ParseContentController : ControllerBase
    {
        private readonly ParseService _parseService;
        private readonly DecodeBase64Service _decodeBase64Service;

        public ParseContentController(ParseService parseService, DecodeBase64Service decodeBase64Service)
        {
            _parseService = parseService;
            _decodeBase64Service = decodeBase64Service;
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult PostPayLoad([FromBody] PayLoadDto payload)
        {
            if (!ModelState.IsValid)
            {
                throw new FormatException("Invalid request payload. Incorrect body format or type");
            }

            if (payload.Type != Enums.CheckType.CSV && payload.Type != Enums.CheckType.InternalJson)
            {
                throw new ArgumentOutOfRangeException("Unsupported type");
            }

            PayLoadDto decodePayLoad = _decodeBase64Service.DecodeBase64(payload);
            ResponseDto response = new ResponseDto();

            switch (decodePayLoad.Type)
            {
                case Enums.CheckType.CSV:
                    response = _parseService.CsvParse(decodePayLoad);
                    break;
                case Enums.CheckType.InternalJson:
                    response = _parseService.InternalJsonParse(decodePayLoad);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(payload.Type), payload.Type, "Unsupported type");
            }

            return Ok(response);
        }
    }
}
