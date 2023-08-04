namespace Artesian.SDK.Dto.GMEPublicOffer
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum Market : byte
    {
        MGP = 0,
        MI1 = 1,
        MI2 = 2,
        MI3 = 3,
        MI4 = 4,
        MI5 = 5,
        MI6 = 6,
        MI7 = 7,
        MIA1 = 8,
        MIA2 = 9,
        MIA3 = 10,
        MSD = 20,
        MB = 30,
        MB2 = 31,
        MB3 = 32,
        MB4 = 33,
        MB5 = 34,
        MB6 = 35,
        MBh = 36,
        MRR = 50,
        MIXBID= 70,
    }

    public enum Status : byte
    {
        ACC = 0,
        REJ = 1,
        INC = 2,
        REP = 3,
        REV = 4,
        SUB = 5,
    }

    public enum Purpose : byte
    {
        BID = 0,
        OFF = 1
    }

    public enum Type : byte
    {
        REG = 0,
        STND = 1
    }

    public enum Scope : byte
    {
        NULL = 0,
        ACC = 1,
        AS = 2,
        CA = 3,
        GR1 = 4,
        GR2 = 5,
        GR3 = 6,
        GR4 = 7,
        RS = 8,
    }

    public enum BAType : byte
    {
        NULL = 0,
        NETT = 1,
        NREV = 2,
        REV = 3,
    }

    public enum Zone : byte
    {
        AUST = 0,
        BRNN = 1,
        CNOR = 2,
        COAC = 3,
        CORS = 4,
        CSUD = 5,
        FOGN = 6,
        FRAN = 7,
        GREC = 8,
        MALT = 9,
        NORD = 10,
        PRGP = 11,
        ROSN = 12,
        SARD = 13,
        SICI = 14,
        SLOV = 15,
        SUD = 16,
        SVIZ = 17,
        CALA = 18,
        MONT = 19,
        XGRE = 20,
        XSVI = 21,
        BSP = 22,
    }

    public enum GenerationType : byte
    {
        UNKNOWN = 0,
        OTHER = 1,
        AUTOGENERATION = 2,
        BIOMASS = 3,
        COAL = 4,
        WIND = 5,
        PV = 6,
        GAS = 7,
        GASOIL = 8,
        THERMAL = 9,
        HYDRO = 10,
        MIXED = 11,
        OIL = 12
    }

    public enum UnitType : byte
    {
        UP = 0,
        UPV = 1,
        UC = 2,
        UCV = 3
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}