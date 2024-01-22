using FE.ADMIN.Models;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FE.ADMIN.Services
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IBaseService _baseService;
        public PhoneNumberService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber"
            });
        }
        public async Task<ResponseDTO?> GetByIDAsync(int ID)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/" + ID
            });
        }

        public async Task<ResponseDTO?> GetByNumberAsync(string Number)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/GetByNumber/" + Number
            });
        }
        public async Task<ResponseDTO?> GetListPhoneBySiteIDAsync(int SiteID)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/GetListPhoneBySiteID/" + SiteID
            });
        }
        public async Task<ResponseDTO?> GetListPhoneByProjectID(string SiteName)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.ApiKM58 + "/api/PhoneNumber/GetListPhoneByProjectID/" + SiteName
            });
        }


        public async Task<ResponseDTO?> Post(PhoneNumberDTO phoneNumberDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Data = phoneNumberDTO,
                Url = SD.ApiKM58 + "/api/PhoneNumber"
            });
        }

        public async Task<ResponseDTO?> Delete(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.DELETE,
                Url = SD.ApiKM58 + "/api/PhoneNumber/Delete/" + id
            });
        }

        public async Task<ResponseDTO?> Edit(PhoneNumberDTO phoneNumberDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.PUT,
                Data = phoneNumberDTO,
                Url = SD.ApiKM58 + "/api/PhoneNumber"
            });
        }

        

       
       
    }
}
