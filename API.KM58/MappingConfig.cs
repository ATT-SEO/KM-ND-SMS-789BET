﻿using API.KM58.Model;
using API.KM58.Model.DTO;
using AutoMapper;

namespace API.KM58
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Site, SiteDTO>().ReverseMap();
                config.CreateMap<LogAccount, LogAccountDTO>().ReverseMap();
                config.CreateMap<PhoneNumber, PhoneNumberDTO>().ReverseMap();
                config.CreateMap<SMS, SMSDTO>().ReverseMap();
                config.CreateMap<SMSRawData, SMSRawDataDTO>().ReverseMap();
                config.CreateMap<AccountRegisters, AccountRegistersDTO>().ReverseMap();
				config.CreateMap<Agent, AgentDTO>().ReverseMap();
			});
            return mappingConfig;
        }
    }
}
