using API.KM58.Model;
using API.KM58.Model.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KM58.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class ReadTimeController : ControllerBase
    {
        private ResponseDTO _response;
        public ReadTimeController()
        {
            _response = new ResponseDTO();
        }
        [HttpGet]
        public ResponseDTO Get()
        {
            _response.Result = new { DateTime = DateTime.Now};
            return _response;
        }
    }
}
