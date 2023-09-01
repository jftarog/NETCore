using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class SongMappings : Profile
    {
        public SongMappings()
        {
            CreateMap<SongEntity, SongDto>().ReverseMap();
            CreateMap<SongEntity, SongUpdateDto>().ReverseMap();
            CreateMap<SongEntity, SongCreateDto>().ReverseMap();
        }
    }
}
