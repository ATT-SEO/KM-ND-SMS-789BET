using API.KM58.Data;
using API.KM58.Model;
using API.KM58.Model.DTO;
using API.KM58.Models;
using API.KM58.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Security.Policy;

namespace API.KM58.Controllers
{
    [Route("api/BOAPI")]
    [ApiController]
    public class BOAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private readonly AppDbContext _db;
        private readonly IBOService _boService;
        public BOAPIController(AppDbContext db, IBOService boService)
        {
            _response = new ResponseDTO();
            _db = db;
            _boService = boService;
        }

        [HttpGet]
        [Route("CheckAccountUserName/{Account}")]
        public async Task<ResponseDTO> CheckAccountUserName(string Account)
        {
            try
            {
                string _account = Account;
                var jsonAllJiLiFishTickets = await _boService.BOGetCheckAccount(_account);
                //var jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result['data'].ToString());
                JObject jsonResponseData = (JObject)JsonConvert.DeserializeObject(jsonAllJiLiFishTickets.Result.ToString());

                JObject resultObject = jsonResponseData["result"] as JObject;
                JArray dataArray = resultObject?["data"] as JArray;
                JObject dataObject = dataArray?.FirstOrDefault() as JObject;
                _response.Result = dataObject;
            }
            catch (Exception Ex)
            {
                _response.IsSuccess = false;
                _response.Message = Ex.Message;
            }
            return _response;
        }
    }
}
