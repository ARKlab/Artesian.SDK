using Artesian.SDK.Dto.GMEPublicOffer;
using NodaTime;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    interface IGMEPublicOfferQuery
    {
        GMEPublicOfferQuery ForDate(LocalDate date);
        GMEPublicOfferQuery ForPurpose(GME_Purpose purpose);
        GMEPublicOfferQuery ForStatus(GME_Status status);
        GMEPublicOfferQuery ForOperator(string[] @operator);
        GMEPublicOfferQuery ForUnit(string[] unit);
        GMEPublicOfferQuery ForMarket(GME_Market[] market);
        GMEPublicOfferQuery ForScope(GME_Scope[] scope);
        GMEPublicOfferQuery ForBAType(GME_BAType[] baType);
        GMEPublicOfferQuery ForZone(GME_Zone[] zone);
        GMEPublicOfferQuery ForUnitType(GME_UnitType[] unitType);
        GMEPublicOfferQuery ForGenerationType(GME_GenerationType[] generationType);
        GMEPublicOfferQuery WithSort(string[] sort);
        GMEPublicOfferQuery WithPagination(int page, int pageSize);
    }
}
