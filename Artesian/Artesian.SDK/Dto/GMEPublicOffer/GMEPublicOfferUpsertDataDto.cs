﻿using MessagePack;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferUpsertDataDto class
    /// </summary>
    [MessagePackObject]
    public class GMEPublicOfferUpsertDataDto
    {
        /// <summary>
        /// GMEPublicOffer
        /// </summary>
        [Key(0)]
        public IList<GMEPublicOfferDto> GMEPublicOffer { get; set; }
        /// <summary>
        /// Flag to choose between syncronoys and asyncronous command execution
        /// </summary>
        [Key(1)]
        public bool DeferCommandExecution { get; set; } = true;

        /// <summary>
        /// Flag to choose between syncronoys and asyncronous precomputed data generation
        /// </summary>
        [Key(2)]
        public bool DeferDataGeneration { get; set; } = true;
    }

    internal static class GMEPublicOfferUpsertDataDtoExt
    {
        public static void Validate(this GMEPublicOfferUpsertDataDto upsertData)
        {
            if (upsertData.GMEPublicOffer == null || upsertData.GMEPublicOffer.Count == 0)
                throw new ArgumentException("'GMEPublicOffer' needs some elements on it");

            foreach (var item in upsertData.GMEPublicOffer)
            {
                if (item.UnitReference == null)
                    throw new ArgumentException("'UnitReference' must be valorized");

                if (item.Operator == null)
                    throw new ArgumentException("'Operator' must be valorized");

                if (item.Data == null || item.Data.Count == 0)
                    throw new ArgumentException("'Data' needs at least one element on it");
            }

        }
    }
}