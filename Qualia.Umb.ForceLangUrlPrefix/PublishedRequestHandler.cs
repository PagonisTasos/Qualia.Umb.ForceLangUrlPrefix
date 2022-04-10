using System.Linq;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Qualia.Umb.ForceLangUrlPrefix
{
    internal class PublishedRequestHandler : INotificationHandler<RoutingRequestNotification>
    {
        private readonly IDomainService domainService;
        private readonly IDefaultCultureAccessor defaultCultureAccessor;

        public PublishedRequestHandler(
            IDomainService domainService
            , IDefaultCultureAccessor defaultCultureAccessor
            )
        {
            this.domainService = domainService;
            this.defaultCultureAccessor = defaultCultureAccessor;
        }
        public void Handle(RoutingRequestNotification notification)
        {
            var requestBuilder = notification.RequestBuilder;

            if (
                requestBuilder.Domain == null
                && domainService?.GetAll(true)?
                    .FirstOrDefault(d => d.LanguageIsoCode == defaultCultureAccessor?.DefaultCulture)
                    is IDomain dom
                )
            {
                requestBuilder.SetRedirectPermanent($"{dom.DomainName}{requestBuilder.AbsolutePathDecoded}");
            }
        }
    }
}
