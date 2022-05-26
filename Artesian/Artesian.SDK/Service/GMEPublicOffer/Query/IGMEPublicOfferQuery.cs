using Artesian.SDK.Dto.GMEPublicOffer;
using NodaTime;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    interface IGMEPublicOfferQuery
    {
        GMEPublicOfferQuery ForDate(LocalDate date);
        GMEPublicOfferQuery ForPurpose(Purpose purpose);
        GMEPublicOfferQuery ForStatus(Status status);
        GMEPublicOfferQuery ForOperator(string[] @operator);
        GMEPublicOfferQuery ForUnit(string[] unit);
        GMEPublicOfferQuery ForMarket(Market[] market);
        GMEPublicOfferQuery ForScope(Scope[] scope);
        GMEPublicOfferQuery ForBAType(BAType[] baType);
        GMEPublicOfferQuery ForZone(Zone[] zone);
        GMEPublicOfferQuery ForUnitType(UnitType[] unitType);
        GMEPublicOfferQuery ForGenerationType(GenerationType[] generationType);
        GMEPublicOfferQuery WithSort(string[] sort);
        GMEPublicOfferQuery WithPagination(int page, int pageSize);
    }
}
