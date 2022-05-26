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
        public Purpose? Purpose { get; set; }
        public Status? Status { get; set; }

        public IEnumerable<string> Operator { get; set; }
        public IEnumerable<string> Unit { get; set; }
        public IEnumerable<Market> Market { get; set; }
        public IEnumerable<Scope> Scope { get; set; }
        public IEnumerable<BAType> BAType { get; set; }
        public IEnumerable<Zone> Zone { get; set; }
        public IEnumerable<UnitType> UnitType { get; set; }
        public IEnumerable<GenerationType> GenerationType { get; set; }
        public IEnumerable<string> Sort { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}
