using MessagePack;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// UnitDto class
    /// </summary>
    [MessagePackObject]
    public class UnitDto
    {
        /// <summary>
        /// Unit Id
        /// </summary>
        [Key(0)]
        public int Id { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [Key(1)]
        public string Unit { get; set; }
    }
}
