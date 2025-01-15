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
                Coalesce = new DerivedCfgCoalesceReadOnly(derivedCfg as DerivedCfgCoalesce);
            else if (derivedCfg is DerivedCfgSum)
                Sum = new DerivedCfgSumReadOnly(derivedCfg as DerivedCfgSum);
            else if (derivedCfg is DerivedCfgMuv)
                Muv = new DerivedCfgMuvReadOnly(derivedCfg as DerivedCfgMuv);
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
