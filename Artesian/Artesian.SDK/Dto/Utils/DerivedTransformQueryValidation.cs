namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Derived transform query validation.
    /// </summary>
    public class DerivedTransformQueryValidation
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
            /// The query string to be validated.
            /// </summary>
            public string? Query { get; init; }

            /// <summary>
            /// The time series data used for the query validation.
            /// </summary>
            public TimeSerieData.V1 Data { get; set; }
        }
    }
}
