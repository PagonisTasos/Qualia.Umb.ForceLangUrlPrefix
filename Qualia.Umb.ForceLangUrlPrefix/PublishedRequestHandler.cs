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
        private bool hasDefaultCulture(IDomain d) => d.LanguageIsoCode == defaultCultureAccessor?.DefaultCulture;
        private IDomain? default_domain => 
            domainService?
            .GetAll(includeWildcards: true)?
            .FirstOrDefault(hasDefaultCulture)
            ;

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
            var request = notification.RequestBuilder;

            if (request.Domain == null && default_domain != null)
            {
                request.SetRedirectPermanent($"{default_domain.DomainName}{request.AbsolutePathDecoded}");
            }
        }
    }
}
