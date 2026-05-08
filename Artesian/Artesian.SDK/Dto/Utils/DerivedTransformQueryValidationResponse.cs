namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents the response of a derived transform query validation.
    /// </summary>
    public class DerivedTransformQueryValidationResponse
    {
        /// <summary>
        /// V1
        /// </summary>
        public record V1
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="V1"/> record.
            /// </summary>
            public V1()
            {
                Data = new TimeSerieData.V1();
            }

            /// <summary>
            /// The query is valid or invalid.
            /// </summary>
            public bool Valid { get; set; }

            /// <summary>
            /// The Error in case of invalid query validation.
            /// </summary>
            public Error? Error { get; set; }

            /// <summary>
            /// The time series data transfored by the query.
            /// </summary>
            public TimeSerieData.V1 Data { get; set; }
        }

        /// <summary>
        /// Represents an error in the derived transform query validation response.
        /// </summary>
        public class Error
        {
            /// <summary>
            /// The Error message when the query validation is invalid.
            /// </summary>
            public string? Message { get; set; }
        }
    }
}
