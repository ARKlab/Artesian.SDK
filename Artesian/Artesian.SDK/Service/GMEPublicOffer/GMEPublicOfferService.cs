using Flurl;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class GMEPublicOfferService : IGMEPublicOfferService
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
            _client = new Client(cfg, ArtesianConstants._gMEPublicOfferRoute.AppendPathSegment(ArtesianConstants._gMEPublicOfferVersion), _policy);
        }

    }
}
