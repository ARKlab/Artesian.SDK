using MessagePack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.SDK.Dto.DerivedCfg
{
    [MessagePackObject]
    public abstract record DerivedCfgWithReferencedIds : DerivedCfgBase
    {
        [Key("OrderedReferencedMarketDataIds")]
        public int[] OrderedReferencedMarketDataIds { get; set; }
    }
}
