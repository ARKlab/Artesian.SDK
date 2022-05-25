using Artesian.SDK.Dto.GMEPublicOffer;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    internal class GMEPublicOfferQueryParamaters
    {
        public GMEPublicOfferQueryParamaters()
        {
        }

        public LocalDate? Date { get; set; }
        public GME_Purpose? Purpose { get; set; }
        public GME_Status? Status { get; set; }

        public IEnumerable<string> Operator { get; set; }
        public IEnumerable<string> Unit { get; set; }
        public IEnumerable<GME_Market> Market { get; set; }
        public IEnumerable<GME_Scope> Scope { get; set; }
        public IEnumerable<GME_BAType> BAType { get; set; }
        public IEnumerable<GME_Zone> Zone { get; set; }
        public IEnumerable<GME_UnitType> UnitType { get; set; }
        public IEnumerable<GME_GenerationType> GenerationType { get; set; }
        public IEnumerable<string> Sort { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}
