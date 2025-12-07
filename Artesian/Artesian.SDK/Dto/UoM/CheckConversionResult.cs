using System.Collections.Generic;

namespace Artesian.SDK.Dto.UoM
{
    /// <summary>
    /// The CheckConversionResult class.
    ///             TargetUnitOfMeasure = the target unit of measure
    ///             ConvertibleInputUnitsOfMeasure = list of convertible input units of measure
    ///             NotConvertibleInputUnitsOfMeasure = list of not convertible input units of measure
    /// </summary>
    public record CheckConversionResult
    {
        /// <summary>
        /// CheckConversionResult Constructor
        /// </summary>
        public CheckConversionResult()
        {
        }

        /// <summary>
        /// CheckConversionResult Constructor
        /// </summary>
        /// <param name="targetUnitOfMeasure"></param>
        public CheckConversionResult(string targetUnitOfMeasure)
        {
            TargetUnitOfMeasure = targetUnitOfMeasure;
        }

        /// <summary>
        /// Add convertible input unit of measure
        /// </summary>
        /// <param name="inputUnitOfMeasure"></param>
        public void AddConvertibleInputUnitOfMeasure(string inputUnitOfMeasure)
        {
            if (!ConvertibleInputUnitsOfMeasure.Contains(inputUnitOfMeasure))
                ConvertibleInputUnitsOfMeasure.Add(inputUnitOfMeasure);
        }

        /// <summary>
        /// Add not convertible input unit of measure
        /// </summary>
        /// <param name="inputUnitOfMeasure"></param>
        public void AddNotConvertibleInputUnitOfMeasure(string inputUnitOfMeasure)
        {
            if (!NotConvertibleInputUnitsOfMeasure.Contains(inputUnitOfMeasure))
                NotConvertibleInputUnitsOfMeasure.Add(inputUnitOfMeasure);
        }

        /// <summary>
        /// TargetUnitOfMeasure
        /// </summary>
        public string TargetUnitOfMeasure { get; set; }
        /// <summary>
        /// ConvertibleInputUnitsOfMeasure
        /// </summary>
        public List<string>? ConvertibleInputUnitsOfMeasure { get; set; } = new List<string>();
        /// <summary>
        /// NotConvertibleInputUnitsOfMeasure
        /// </summary>
        public List<string>? NotConvertibleInputUnitsOfMeasure { get; set; } = new List<string>();
    }
}
