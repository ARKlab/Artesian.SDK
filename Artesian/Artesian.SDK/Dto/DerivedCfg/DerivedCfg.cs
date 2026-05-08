
namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The DerivedCfg containing all types of derived configurations (DerivedCfgCoalesce, DerivedCfgSum, DerivedCfgMuv, DerivedCfgTransform)
    /// </summary>
    public record DerivedCfg
    {
        /// <summary>
        /// DerivedCfg constructor
        /// </summary>
        /// <param name="derivedCfg"></param>
        internal DerivedCfg(DerivedCfgBase derivedCfg)
        {
            if (derivedCfg is DerivedCfgCoalesce coalesce)
                Coalesce = new DerivedCfgCoalesceReadOnly(coalesce);
            else if (derivedCfg is DerivedCfgSum sum)
                Sum = new DerivedCfgSumReadOnly(sum);
            else if (derivedCfg is DerivedCfgMuv muv)
                Muv = new DerivedCfgMuvReadOnly(muv);
            else if (derivedCfg is DerivedCfgTransform transform)
                Transform = new DerivedCfgTransformReadOnly(transform);
        }

        /// <summary>
        /// DerivedCfgTransform
        /// </summary>
        public DerivedCfgTransformReadOnly? Transform { get; protected set; } = null;

        /// <summary>
        /// DerivedCfgCoalesce
        /// </summary>
        public DerivedCfgCoalesceReadOnly? Coalesce { get; protected set; } = null;

        /// <summary>
        /// DerivedCfgSum
        /// </summary>
        public DerivedCfgSumReadOnly? Sum { get; protected set; } = null;

        /// <summary>
        /// DerivedCfgMuv
        /// </summary>
        public DerivedCfgMuvReadOnly? Muv { get; protected set; } = null;

    }
}
