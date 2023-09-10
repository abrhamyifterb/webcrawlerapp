using HotChocolate.Types;

public class NodeType : ObjectType<Node>
{
    protected override void Configure(IObjectTypeDescriptor<Node> descriptor)
    {
        descriptor.Field(t => t.Title)
            .Type<StringType>();

        descriptor.Field(t => t.Url)
            .Type<NonNullType<StringType>>();

        descriptor.Field(t => t.CrawlTime)
            .Type<StringType>();

        descriptor.Field(t => t.Links)
            .Type<ListType<NodeType>>();

        // descriptor.Field(t => t.Owner)
        //     .Type<NonNullType<WebPageType>>();
    }
}
