using AlgimedApp.Data.Models;
using AlgimedApp.Shared.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Shared.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginDto, User>().ReverseMap();

            CreateMap<ModeDto, Mode>()
     .ForMember(dest => dest.ID, opt => opt.Ignore()) 
     .ReverseMap();

            CreateMap<StepDto, Step>()
    .ForMember(dest => dest.ID, opt => opt.Ignore()) 
    .ReverseMap();
        }
    }
}