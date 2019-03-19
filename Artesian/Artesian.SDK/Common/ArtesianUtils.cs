﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Service;
using System;
using Artesian.SDK.Dto;
using System.Text.RegularExpressions;
using System.IO;

namespace Artesian.SDK.Common
{
    /// <summary>
    /// Artesian Utils class for validation
    /// </summary>
    public class ArtesianUtils
    {
        /// <summary>
        /// Mapping the Date Period based on Granularity
        /// </summary>
        /// <param name="granularity">Granularity</param>
        public static DatePeriod MapDatePeriod(Granularity granularity)
        {
            DatePeriod selectedPeriod = DatePeriod.Day;

            switch (granularity)
            {
                case Granularity.Week:
                    {
                        selectedPeriod = DatePeriod.Week;
                        break;
                    }
                case Granularity.Month:
                    {
                        selectedPeriod = DatePeriod.Month;
                        break;
                    }
                case Granularity.Quarter:
                    {
                        selectedPeriod = DatePeriod.Trimestral;
                        break;
                    }
                case Granularity.Year:
                    {
                        selectedPeriod = DatePeriod.Calendar;
                        break;
                    }
            }

            return selectedPeriod;
        }

        /// <summary>
        /// Mapping the Time Period based on Granularity
        /// </summary>
        /// <param name="granularity">Granularity</param>
        public static TimePeriod MapTimePeriod(Granularity granularity)
        {
            if (!granularity.IsTimeGranularity())
                throw new ArgumentException("not a time granularity", nameof(granularity));

            TimePeriod selectedPeriod = TimePeriod.Hour;

            switch (granularity)
            {
                case Granularity.Hour:
                    {
                        selectedPeriod = TimePeriod.Hour;
                        break;
                    }
                case Granularity.ThirtyMinute:
                    {
                        selectedPeriod = TimePeriod.HalfHour;
                        break;
                    }
                case Granularity.FifteenMinute:
                    {
                        selectedPeriod = TimePeriod.QuarterHour;
                        break;
                    }
                case Granularity.TenMinute:
                    {
                        selectedPeriod = TimePeriod.TenMinutes;
                        break;
                    }
                case Granularity.Minute:
                    {
                        selectedPeriod = TimePeriod.Minute;
                        break;
                    }
            }

            return selectedPeriod;
        }

        /// <summary>
        /// Is valid string checks to see if the string provided is between the provided min and max length
        /// </summary>
        /// <param name="validStringCheck">string</param>
        /// <param name="minLenght">int</param>
        /// <param name="maxLenght">int</param>
        public static void IsValidString(string validStringCheck, int minLenght, int maxLenght)
        {
            if (String.IsNullOrEmpty(validStringCheck))
                throw new ArgumentException("Provider null or empty exception");
            if (validStringCheck.Length < minLenght || validStringCheck.Length > maxLenght)
                throw new ArgumentException("Provider must be between 1 and 50 characters.");
            if (!new Regex(ArtesianConstants.CharacterValidatorRegEx).Match(validStringCheck).Success)
                throw new ArgumentException($"Invalid string '{validStringCheck}'.Should not contain trailing or leading whitespaces or any of the following characters: ,:; '\"<space>");
        }
        /// <summary>
        /// Is valid provider
        /// </summary>
        /// <param name="provider">string</param>
        /// <param name="minLenght">int</param>
        /// <param name="maxLenght">int</param>
        public static void IsValidProvider(string provider, int minLenght, int maxLenght)
        {
            IsValidString(provider, minLenght, maxLenght);
        }
        /// <summary>
        /// Is valid Market Data name
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="minLenght">int</param>
        /// <param name="maxLenght">int</param>
        public static void IsValidMarketDataName(string name, int minLenght, int maxLenght)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("MarketData name must not be null or empty");
            if (name.Length < minLenght || name.Length > maxLenght)
                throw new ArgumentException("MarketData name must be between 1 and 250 characters.");
            if (!new Regex(ArtesianConstants.MarketDataNameValidatorRegEx).Match(name).Success)
                throw new ArgumentException($"Invalid string '{name}'. Should not contain trailing or leading whitespaces and no other whitespace than <space> in the middle.");
        }
    }
}
