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
            if (derivedCfg is DerivedCfgCoalesce)
                DerivedCfgCoalesce = new DerivedCfgCoalesceReadOnly(derivedCfg as DerivedCfgCoalesce);
            else if (derivedCfg is DerivedCfgSum)
                DerivedCfgSum = new DerivedCfgSumReadOnly(derivedCfg as DerivedCfgSum);
            else if (derivedCfg is DerivedCfgMuv)
                DerivedCfgMuv = new DerivedCfgMuvReadOnly(derivedCfg as DerivedCfgMuv);
        }

        /// <summary>
        /// DerivedCfgCoalesce
        /// </summary>
        public DerivedCfgCoalesceReadOnly? DerivedCfgCoalesce { get; protected set; } = null;

        /// <summary>
        /// DerivedCfgSum
        /// </summary>
        public DerivedCfgSumReadOnly? DerivedCfgSum { get; protected set; } = null;

        /// <summary>
        /// DerivedCfgMuv
        /// </summary>
        public DerivedCfgMuvReadOnly? DerivedCfgMuv { get; protected set; } = null;

    }
}
