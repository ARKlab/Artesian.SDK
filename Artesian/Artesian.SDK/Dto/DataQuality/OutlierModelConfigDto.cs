using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.DataQuality.Serialize;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Abstract base class for outlier detection model configurations.
    /// Each concrete subtype represents a different statistical approach to identify anomalous data points.
    /// Available models: <see cref="OutlierAbsoluteBoundConfigDto"/>, <see cref="OutlierRefCurveConfigDto"/>.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(OutlierAbsoluteBoundConfigDto))]
    [Union(1, typeof(OutlierRefCurveConfigDto))]
    [System.Text.Json.Serialization.JsonConverter(typeof(OutlierModelConfigConverterSTJ))]
    public abstract class OutlierModelConfigDto
    {
        /// <summary>
        /// Discriminator property identifying the outlier detection model type.
        /// </summary>
        [IgnoreMember]
        public abstract OutlierModel Model { get; }
    }
}
