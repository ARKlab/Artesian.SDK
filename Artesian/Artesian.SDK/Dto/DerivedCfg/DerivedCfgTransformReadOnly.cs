using Artesian.SDK.Dto.Enums;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// DerivedMuv Configuration Readonly
    /// </summary>
    public record DerivedCfgTransformReadOnly : DerivedCfgTransform
    {
        /// <summary>
        /// DerivedTransform constructor
        /// </summary>
        /// <param name="cfg"></param>
        public DerivedCfgTransformReadOnly(DerivedCfgTransform cfg)
        {
            // Copy properties from the original cfg to the read-only version
            this.Version = cfg.Version;
            this.DerivedAlgorithm = cfg.DerivedAlgorithm;
            this.Transform = cfg.Transform;
            // Copy other properties as needed
        }

        /// <summary>
        /// DerivedAlgorithm
        /// </summary>
        public new DerivedAlgorithm DerivedAlgorithm { get; }
        /// <summary>
        /// Version
        /// </summary>
        public new int Version { get; }
        /// <summary>
        /// Transform query
        /// </summary>
        public new string? Transform { get; set; }

    }
}
