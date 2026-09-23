using ContentParserApi.DTOs;
using ContentParserApi.Validations;
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
        private readonly Validation _validate;

        public ParseContentController(ParseService parseService, DecodeBase64Service decodeBase64Service, Validation validate)
        {
            _parseService = parseService;
            _decodeBase64Service = decodeBase64Service;
            _validate = validate;
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult PostPayLoad([FromBody] PayLoadDto payload)
        {
            _validate.Validate(payload);
            var decodePayLoad = _decodeBase64Service.DecodeBase64(payload);
            var response = _parseService.GetParse(decodePayLoad);

            return Ok(response);   
        }
    }
}
