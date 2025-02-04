
namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Block Type, for specifing the bid's block type
    /// </summary>
    public enum BlockType : byte
    {
        /// <summary>
        /// Single - bid/offer refers to a single BidTimestamp
        /// </summary>
        Single = 0,
        /// <summary>
        /// Block - bid/offer is part of a block, referencing multiple contiguous BidTimestamp
        /// </summary>
        Block = 1
    }
}
