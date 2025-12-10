using MessagePack;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// UnitConfiguration class
    /// </summary>
    [MessagePackObject]
    public record UnitConfigurationDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitConfigurationDto"/> class.
        /// </summary>
        /// <param name="unit">Unit name</param>
        [SerializationConstructor]
        public UnitConfigurationDto(string unit)
        {
            Unit = unit;
            Mappings = new();
        }

        /// <summary>
        /// Unit name
        /// </summary>
        [Key(0)]
        public string Unit { get; init; }
        /// <summary>
        /// Generation type mappings
        /// </summary>
        [Key(1)]
        public List<GenerationTypeMapping> Mappings { get; init; }

        /// <summary>
        /// ETag
        /// </summary>
        [Key(2)]
        public string? ETag { get; init; }
    }


    /// <summary>
    /// GenerationTypeMapping class
    /// </summary>
    [MessagePackObject]
    public record GenerationTypeMapping
    {
        /// <summary>
        /// GenerationType
        /// </summary>
        [Key(0)]
        public GenerationType GenerationType { get; init; }

        /// <summary>
        /// From date
        /// </summary>
        [Key(1)]
        public LocalDate From { get; init; }

        /// <summary>
        /// To date
        /// </summary>
        [Key(2)]
        public LocalDate To { get; init; }
    }
}
