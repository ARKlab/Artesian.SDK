using Artesian.SDK.Dto.GMEPublicOffer;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    internal sealed record GMEPublicOfferQueryParamaters
    {
        public LocalDate? Date { get; set; }
        public Purpose? Purpose { get; set; }
        public Status? Status { get; set; }

        public IEnumerable<string> Operator { get; set; } = null!;

        public IEnumerable<string> Unit { get; set; } = null!;

        public IEnumerable<Market> Market { get; set; } = null!;

        public IEnumerable<Scope> Scope { get; set; } = null!;

        public IEnumerable<BAType> BAType { get; set; } = null!;

        public IEnumerable<Zone> Zone { get; set; } = null!;

        public IEnumerable<UnitType> UnitType { get; set; } = null!;

        public IEnumerable<GenerationType> GenerationType { get; set; } = null!;

        public IEnumerable<string> Sort { get; set; } = null!;

        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}
