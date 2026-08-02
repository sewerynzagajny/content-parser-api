using ContentParserApi.DTOs;
using ContentParserApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ContentParserApi.Controllers
{
    [Route("api/v1/parse-content")]
    [ApiController]
    public class ParseContentController : ControllerBase
    {
    private readonly ParseService _parseService;
    private readonly DecodeBase64Service _decodeBase64Service;


    public ParseContentController (ParseService parseService, DecodeBase64Service decodeBase64Service)
    {
    _parseService = parseService;
    _decodeBase64Service = decodeBase64Service;
    }

       static private ResponseDto BuildErrorResponse(string massage)
        {
            ResponseDto errorResponse = new ResponseDto
            {
                Status = "Error",
                DataProcessed = massage,
            };

            return errorResponse;
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult PostPayLoad([FromBody] PayLoadDto payload) 
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(BuildErrorResponse("Invalid request payload. Incorrect body format or type"));
            }

            try
            {
                ResponseDto parsedData = new ResponseDto();
                ResponseDto response = new ResponseDto();

                switch (payload.Type)
                {
                    case Enums.CheckType.CSV:
                        parsedData = _parseService.CsvParse(payload);
                        response = _decodeBase64Service.DecodeBase64(parsedData);
                        break;
                    case Enums.CheckType.InternalJson:
                        parsedData = _parseService.InternalJsonParse(payload);
                        response = _decodeBase64Service.DecodeBase64(parsedData);
                        break;
                    default:
                       return BadRequest (BuildErrorResponse("Unsupported type"));
                }
                return Ok(response);

            }
            catch (ArgumentException ex)
            {
               return BadRequest(BuildErrorResponse(ex.Message));
            }

            catch (FormatException ex)
            {
                return BadRequest(BuildErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BuildErrorResponse(ex.Message));
            }


        }

    }
}
