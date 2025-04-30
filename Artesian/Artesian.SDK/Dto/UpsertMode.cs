
namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Upsert Mode, for specifing the type of upsert to be carried out
    /// </summary>
    public enum UpsertMode
    {
        /// <summary>
        /// Merge - merge the data in the payload with the existing data
        /// This is the default opertation for VersionedTimeSerie, BidAsk and Market Assessment type market data
        /// </summary>
        Merge = 0,
        /// <summary>
        /// Replace - replace the existing data with the data in the payload
        /// This is the default operation for ActualTimeSerie and Auction type market data
        /// </summary>
        Replace = 1
    }
}
