using Flurl;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferService class
    /// Contains query types to be created
    /// </summary>
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public partial class GMEPublicOfferService : IGMEPublicOfferService
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private readonly IArtesianServiceConfig _cfg;
        private readonly ArtesianPolicyConfig _policy;
        private readonly Client _client;

        /// <summary>
        /// GME Public offer service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public GMEPublicOfferService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// GME Public offer service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public GMEPublicOfferService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants.GMEPublicOfferRoute.AppendPathSegment(ArtesianConstants.GMEPublicOfferVersion), _policy);
        }

    }
}
