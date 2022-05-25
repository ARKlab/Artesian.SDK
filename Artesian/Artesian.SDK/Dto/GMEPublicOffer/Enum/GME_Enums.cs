namespace Artesian.SDK.Dto.GMEPublicOffer
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum GME_Market : byte
    {
        MGP = 0,
        MI1 = 1,
        MI2 = 2,
        MI3 = 3,
        MI4 = 4,
        MI5 = 5,
        MI6 = 6,
        MI7 = 7,
        MSD = 20,
        MB = 30,
        MB2 = 31,
        MB3 = 32,
        MB4 = 33,
        MB5 = 34,
        MB6 = 35,
        MRR = 50,
    }

    public enum GME_Status : byte
    {
        ACC = 0,
        REJ = 1,
        INC = 2,
        REP = 3,
        REV = 4,
        SUB = 5,
    }

    public enum GME_Purpose : byte
    {
        BID = 0,
        OFF = 1
    }

    public enum GME_Type : byte
    {
        REG = 0,
        STND = 1
    }

    public enum GME_Scope : byte
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

    public enum GME_BAType : byte
    {
        NULL = 0,
        NETT = 1,
        NREV = 2,
        REV = 3,
    }

    public enum GME_Zone : byte
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
    }

    public enum GME_GenerationType : byte
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

    public enum GME_UnitType : byte
    {
        UP = 0,
        UPV = 1,
        UC = 2,
        UCV = 3
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}