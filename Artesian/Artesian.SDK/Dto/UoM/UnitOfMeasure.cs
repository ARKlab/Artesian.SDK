using MessagePack;

namespace Artesian.SDK.Dto.UoM
{
    /// <summary>
    /// UnitOfMeasure Entity
    /// </summary>
    [MessagePackObject]
    public record UnitOfMeasure
    {
        /// <summary>
        /// The UnitOfMeasure
        /// </summary>
        [Key("Value")]
        public string Value { get; set; } = null!;

    }
}
