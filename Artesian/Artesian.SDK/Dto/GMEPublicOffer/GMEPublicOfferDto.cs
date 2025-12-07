using MessagePack;

using System.Collections.Generic;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferDto class
    /// </summary>
    [MessagePackObject]
    public class GMEPublicOfferDto
    {
        /// <summary>
        /// The Purpose
        /// </summary>
        [Key(0)]
        public Purpose? Purpose { get; set; }

        /// <summary>
        /// The Type
        /// </summary>
        [Key(1)]
        public Type? Type { get; set; }

        /// <summary>
        /// The Status
        /// </summary>
        [Key(2)]
        public Status? Status { get; set; }

        /// <summary>
        /// The Market
        /// </summary>
        [Key(3)]
        public Market? Market { get; set; }

        /// <summary>
        /// The Unit Type
        /// </summary>
        [Key(4)]
        public string UnitReference { get; set; }

        /// <summary>
        /// The Zone
        /// </summary>
        [Key(5)]
        public Zone? Zone { get; set; }

        /// <summary>
        /// The Operator
        /// </summary>
        [Key(6)]
        public string Operator { get; set; }

        /// <summary>
        /// The Scope
        /// </summary>
        [Key(7)]
        public Scope? Scope { get; set; }

        /// <summary>
        /// The BAType
        /// </summary>
        [Key(8)]
        public BAType? BAType { get; set; }


        //--------------------------

        /// <summary>
        /// Data
        /// </summary>
        [Key(10)]
        public IList<GMEPublicOfferDataDto>? Data { get; set; }

        //---------------------------

        //Unused Data

        /// <summary>
        /// MARKET_PARTECIPANT_XREF_NO
        /// </summary>
        [Key(11)]
        public string MARKET_PARTECIPANT_XREF_NO { get; set; }

        /// <summary>
        /// BALANCED_REFERENCE_NO
        /// </summary>
        [Key(12)]
        public string BALANCED_REFERENCE_NO { get; set; }

        /// <summary>
        /// CODICE_OFFERTA
        /// </summary>
        [Key(13)]
        public int? CODICE_OFFERTA { get; set; }

        /// <summary>
        /// STORAGE_SOURCE
        /// </summary>
        [Key(14)]
        public string STORAGE_SOURCE { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        [Key(15)]
        public string Filename { get; set; }

        /// <summary>
        /// Granularity
        /// </summary>
        [Key(16)]
        public string Granularity { get; set; }

        /// <summary>
        /// Offer Type
        /// </summary>
        [Key(17)]
        public string OfferType { get; set; }

        /// <summary>
        /// Block ID
        /// </summary>
        [Key(18)]
        public string BlockId { get; set; }

        /*
     [GME_PurposeEnum] [PurposeName]      |   [PURPOSE_CD] [nvarchar](10) NOT NULL
     [GME_TypeEnum]  [TypeName]           | , [TYPE_CD] [nvarchar](10) NULL
     [GME_StatusEnum] [StatusName]        | , [STATUS_CD] [nvarchar](10) NULL
     [GME_MarketEnum] [MarketName]        | , [MARKET_CD] [nvarchar](10) NULL
     [GME_Unit] [Unit]                    | , [UNIT_REFERENCE_NO] [nvarchar](255) NULL
     [GME_ZoneEnum] [ZoneName]            | , [ZONE_CD] [nvarchar](10) NULL
     [GME_Operator] [Operator]            | , [OPERATORE] [nvarchar](255) NULL
     [GME_ScopeEnum] [ScopeName]          | , [SCOPE] [nvarchar](10) NULL
     [GME_BATypeEnum] [BATypeName]        | , [BAType] [nvarchar](255) NULL
     // not used                          | , [MARKET_PARTECIPANT_XREF_NO] [nvarchar](255) NULL      
     // not used                          | , [BALANCED_REFERENCE_NO] [nvarchar](255) NULL
     // not used                          | , [CODICE_OFFERTA] [int] NULL
     // not used                          | , [STORAGE_SOURCE] [nvarchar](255) NULL
     // not used                          | , [Filename] [nvarchar](255) NULL
        
     [GME_Data] [Date]                    | , [BID_OFFER_DATE_DT] [datetime] NULL
     [GME_Data] [Hour]                    | , [INTERVAL_NO] [int] NULL
     [GME_Data] [Quarter]                 | , [QUARTER_NO] [int] NULL
     [GME_Data] [Quantity]                | , [QUANTITY_NO] [decimal](28, 10) NOT NULL
     [GME_Data] [AwardedQuantity]         | , [AWARDED_QUANTITY_NO] [decimal](28, 10) NOT NULL
     [GME_Data] [AwardedPrice]            | , [AWARDED_PRICE_NO] [decimal](28, 10) NULL
     [GME_Data] [EnergyPrice]             | , [ENERGY_PRICE_NO] [decimal](28, 10) NOT NULL
     [GME_Data] [MeritOrder]              | , [MERIT_ORDER_NO] [decimal](28, 10) NOT NULL
     [GME_Data] [PartialQuantityAccepted] | , [PARTIAL_QTY_ACCEPTED_IN] [nvarchar](255) NOT NULL
     [GME_Data] [ADJQuantity]             | , [ADJ_QUANTITY_NO] [decimal](28, 10) NOT NULL
     [GME_Data] [ADJEnergyPrice]          | , [ADJ_ENERGY_PRICE_NO] [decimal](28, 10) NOT NULL  

     [GME_Data] [TransactionReference]    | , [TRANSACTION_REFERENCE_NO] [nvarchar](100) NOT NULL
     [GME_Data] [GridSupplyPoint]         | , [GRID_SUPPLY_POINT_NO] [nvarchar](255) NULL
     [GME_Data] [Bilateral]               | , [BILATERAL_IN] [bit] NULL
     [GME_Data] [SubmittedAt]             | , [SUBMITTED_DT] [datetime] NULL
     [GME_Data] [Timestamp]               | , [TIMESTAMP] [datetime] NULL (only XBID market)
     [GME_Data] [PrezzoUnitario]          | , [PREZZO_UNITARIO] [decimal] NULL (only XBID market)
        */
    }
}