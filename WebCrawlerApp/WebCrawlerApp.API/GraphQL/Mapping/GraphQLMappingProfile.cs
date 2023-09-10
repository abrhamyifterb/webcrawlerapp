using AutoMapper;
using Newtonsoft.Json;
using WebCrawlerApp.Core.Entities;

public class GraphQLMappingProfile : Profile
{
    public GraphQLMappingProfile()
    {
        CreateMap<Website, WebPage>()
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
            .ForMember(dest => dest.Regexp, opt => opt.MapFrom(src => src.BoundaryRegExp))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToList()))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.IsActive));

    //     CreateMap<string, Node>()
    //         .ConvertUsing((src, dest, context) =>
    //         {
    //             var crawledNode = JsonConvert.DeserializeObject<CrawledData>(src);
    //             var nodeDto = new Node
    //             {
    //                 Title = crawledNode.Title,
    //                 Url = crawledNode.Url,
    //                 CrawlTime = crawledNode.CrawlTime,
    //                 Links = crawledNode.Links.Select(link => new Node { Url = link }).ToList(),
    //                 Owner = context.Items["Owner"] as WebPage
    //             };
    //             return nodeDto;
    // });

    }
}
