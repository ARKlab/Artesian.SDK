using MessagePack;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// UnitConfiguration class
    /// </summary>
    [MessagePackObject]
    public class UnitConfigurationDto
    {
        /// <summary>
        /// Unit name
        /// </summary>
        [Key(0)]
        public string Unit { get; set; }
        /// <summary>
        /// Generation type mappings
        /// </summary>
        [Key(1)]
        public List<GenerationTypeMapping> Mappings { get; set; }

        /// <summary>
        /// ETag
        /// </summary>
        [Key(2)]
        public string ETag { get; set; }
    }


    /// <summary>
    /// GenerationTypeMapping class
    /// </summary>
    [MessagePackObject]
    public class GenerationTypeMapping
    {
        /// <summary>
        /// GenerationType
        /// </summary>
        [Key(0)]
        public GenerationType GenerationType { get; set; }

        /// <summary>
        /// From date
        /// </summary>
        [Key(1)]
        public LocalDate From { get; set; }

        /// <summary>
        /// To date
        /// </summary>
        [Key(2)]
        public LocalDate To { get; set; }
    }
}
