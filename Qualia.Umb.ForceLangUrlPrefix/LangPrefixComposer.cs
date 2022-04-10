using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Qualia.Umb.ForceLangUrlPrefix
{
    public class LangPrefixComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<RoutingRequestNotification, PublishedRequestHandler>();
        }
    }
}
