namespace Artesian.SDK.Service.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class GMEPublicOfferService
    {
        /// <summary>
        /// Create GME Public Offer Query
        /// </summary>
        /// <returns>
        /// GME Public Offer Query <see cref="GMEPublicOfferQuery"/>
        /// </returns>
        public GMEPublicOfferQuery CreateRawCurveQuery()
        {
            return new GMEPublicOfferQuery(_client);
        }
    }
}
