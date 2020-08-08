using Grand.Core.Caching;
using Grand.Core.Domain.Configuration;
using Grand.Core.Domain.Localization;
using Grand.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.SwiperSlider.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        //languages
        INotificationHandler<EntityInserted<Language>>,
        INotificationHandler<EntityUpdated<Language>>,
        INotificationHandler<EntityDeleted<Language>>,
        //settings
        INotificationHandler<EntityInserted<Setting>>,
        INotificationHandler<EntityUpdated<Setting>>,
        INotificationHandler<EntityDeleted<Setting>>
    {
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : picture id
        /// </remarks>
        public const string PICTURE_URL_MODEL_KEY = "grand.plugins.widgets.swiperslider.pictureurl-{0}";
        public const string PICTURE_URL_PATTERN_KEY = "grand.plugins.widgets.swiperslider";

        /// <summary>
        /// Key for ManufacturerNavigationModel caching
        /// </summary>
        /// <remarks>
        /// {0} : current manufacturer id
        /// {1} : language id
        /// {2} : roles of the current user
        /// {3} : current store ID
        /// </remarks>
        public const string MANUFACTURER_BRAND_MODEL_KEY = "grand.plugins.widgets.swiperslider.manufacturer.brand-{0}-{1}-{2}-{3}";
        public const string MANUFACTURER_BRAND_PATTERN_KEY = "grand.plugins.widgets.swiperslider.manufacturer.brand";


        /// <summary>
        /// Key for ProductSpecificationModel caching
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : language id
        /// </remarks>
        public const string PRODUCT_SPECS_MODEL_KEY = "grand.plugins.widgets.swiperslider.product.specs-{0}-{1}";
        public const string PRODUCT_SPECS_PATTERN_KEY = "grand.plugins.widgets.swiperslider.product.specs";


        /// <summary>
        /// Key for default product picture caching (all pictures)
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : picture size
        /// {2} : isAssociatedProduct?
        /// {3} : language ID ("alt" and "title" can depend on localized product name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string PRODUCT_DEFAULTPICTURE_MODEL_KEY = "grand.plugins.widgets.swiperslider.product.detailspictures-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string PRODUCT_DEFAULTPICTURE_PATTERN_KEY = "grand.plugins.widgets.swiperslider.product.detailspictures";


        /// <summary>
        /// Key for TopicModel caching
        /// </summary>
        /// <remarks>
        /// {0} : topic system name
        /// {1} : language id
        /// </remarks>
        public const string TOPIC_SENAME_BY_SYSTEMNAME = "grand.plugins.widgets.swiperslider.topic.sename.bysystemname-{0}-{1}";
        public const string TOPIC_PATTERN_KEY = "grand.plugins.widgets.swiperslider.topic";

        private readonly ICacheManager _cacheManager;

        public ModelCacheEventConsumer(ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        public async Task Handle(EntityInserted<Language> eventMessage, CancellationToken cancellationToken)
        {
            //clear all localizable models
            await _cacheManager.RemoveByPrefix(MANUFACTURER_BRAND_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCT_SPECS_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(TOPIC_PATTERN_KEY);
        }


        public async Task Handle(EntityUpdated<Language> eventMessage, CancellationToken cancellationToken)
        {
            //clear all localizable models
            await _cacheManager.RemoveByPrefix(MANUFACTURER_BRAND_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCT_SPECS_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(TOPIC_PATTERN_KEY);
        }

        public async Task Handle(EntityDeleted<Language> eventMessage, CancellationToken cancellationToken)
        {
            //clear all localizable models
            await _cacheManager.RemoveByPrefix(MANUFACTURER_BRAND_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCT_SPECS_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(TOPIC_PATTERN_KEY);
        }

        public async Task Handle(EntityInserted<Setting> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(PICTURE_URL_PATTERN_KEY);

        }
        public async Task Handle(EntityUpdated<Setting> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(PICTURE_URL_PATTERN_KEY);

        }
        public async Task Handle(EntityDeleted<Setting> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(PICTURE_URL_PATTERN_KEY);
        }
    }
}
