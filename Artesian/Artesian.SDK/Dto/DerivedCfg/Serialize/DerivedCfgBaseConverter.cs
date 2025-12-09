using Artesian.SDK.Dto.Enums;

using System;

namespace Artesian.SDK.Dto.Serialize
{
    sealed class DerivedCfgBaseConverterSTJ : JsonPolymorphicConverter<DerivedCfgBase, DerivedAlgorithm>
    {
        public DerivedCfgBaseConverterSTJ()
            : base(nameof(DerivedCfgBase.DerivedAlgorithm))
        {
        }

        protected override Type GetType(DerivedAlgorithm discriminatorValue)
        {
            return discriminatorValue switch
            {
                DerivedAlgorithm.MUV => typeof(DerivedCfgMuv),
                DerivedAlgorithm.Coalesce => typeof(DerivedCfgCoalesce),
                DerivedAlgorithm.Sum => typeof(DerivedCfgSum),
                _ => typeof(DerivedCfgBase)
            };
        }
    }
}
