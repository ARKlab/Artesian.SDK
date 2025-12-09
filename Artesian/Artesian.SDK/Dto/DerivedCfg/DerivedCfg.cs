namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The DerivedCfg containing the three type of derived configurations (DerivedCfgCoalesce, DerivedCfgSum, DerivedCfgMuv)
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
        }

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
