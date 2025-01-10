using Artesian.SDK.Dto.Enums;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// DerivedMuv Configuration Readonly
    /// </summary>
    public record DerivedCfgMuvReadOnly : DerivedCfgMuv
    {
        /// <summary>
        /// DerivedCoalesce constructor
        /// </summary>
        /// <param name="cfg"></param>
        public DerivedCfgMuvReadOnly(DerivedCfgMuv cfg)
        {
            // Copy properties from the original cfg to the read-only version
            this.Version = cfg.Version;
            this.DerivedAlgorithm = cfg.DerivedAlgorithm;
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

    }
}
