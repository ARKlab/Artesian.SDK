using MessagePack;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// OperatorDto class
    /// </summary>
    [MessagePackObject]
    public class OperatorDto
    {
        /// <summary>
        /// Operator Id
        /// </summary>
        [Key(0)]
        public int Id { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        [Key(1)]
        public string Operator { get; set; } = null!;

    }
}
