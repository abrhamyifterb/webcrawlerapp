using HotChocolate.Types;

public class WebNodeType : ObjectType<WebNode>
{
    protected override void Configure(IObjectTypeDescriptor<WebNode> descriptor)
    {

        descriptor.Field(t => t.Title)
            .Type<NonNullType<StringType>>();


        descriptor.Field(t => t.Url)
            .Type<NonNullType<StringType>>();

        descriptor.Field(t => t.CrawlTime)
            .Type<StringType>();

        descriptor.Field(t => t.Links)
            .Type<ListType<NodeType>>();
    }

}
