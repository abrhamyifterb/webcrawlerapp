using AutoMapper;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Application.Dtos;

namespace WebCrawlerApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WebsiteDTO, Website>().ReverseMap();
            CreateMap<ExecutionDTO, Execution>().ReverseMap();
            // CreateMap<CrawledDataDTO, CrawledData>().ReverseMap();
            CreateMap<CrawledDataDTO, CrawledData>().ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.Links ?? new List<string>())).ReverseMap();

        }
    }
}
