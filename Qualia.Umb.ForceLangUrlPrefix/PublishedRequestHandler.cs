using System.Linq;
using Umbraco.Cms.Core.Events;
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

            if (requestBuilder.Domain == null)
            {
                var domains = domainService.GetAll(true);
                var dom = domains.FirstOrDefault(d => d.LanguageIsoCode == defaultCultureAccessor.DefaultCulture);
                requestBuilder.SetRedirectPermanent($"{dom.DomainName}{requestBuilder.AbsolutePathDecoded}");
            }
        }
    }
}
