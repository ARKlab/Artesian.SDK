
namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Block Type for specifing the the bid block type
    /// </summary>
    public enum BlockType : byte
    {
        /// <summary>
        /// Single bid/offer consists of a specified volume and price for a specific time of the day
        /// </summary>
        Single = 0,
        /// <summary>
        /// Block bid/offer consists of a specified volume and price for a certain number of consecutive times within the same day
        /// </summary>
        Block = 1
    }
}
