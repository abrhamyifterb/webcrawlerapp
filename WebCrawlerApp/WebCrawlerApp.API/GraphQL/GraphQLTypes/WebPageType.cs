using HotChocolate.Types;

public class WebPageType : ObjectType<WebPage>
{
    protected override void Configure(IObjectTypeDescriptor<WebPage> descriptor)
    {
        descriptor.Field(t => t.Identifier)
            .Type<NonNullType<IdType>>();

        descriptor.Field(t => t.Label)
            .Type<NonNullType<StringType>>();

        descriptor.Field(t => t.Url)
            .Type<NonNullType<StringType>>();

        descriptor.Field(t => t.Regexp)
            .Type<NonNullType<StringType>>();

        descriptor.Field(t => t.Tags)
            .Type<ListType<StringType>>();

        descriptor.Field(t => t.Active)
            .Type<NonNullType<BooleanType>>();
    }
}
