using System.Collections.Generic;

namespace Artesian.SDK.Dto.UnitOfMeasure
{
    /// <summary>
    /// The CheckConversionResults class.
    ///             TargetUnitOfMeasure = the target unit of measure
    ///             ConvertibleInputUnitOfMeasure = list of convertible input unit of measure
    ///             NotConvertibleInputUnitOfMeasure = list of not convertible input unit of measure
    /// </summary>
    public record CheckConversionResults
    {
        /// <summary>
        /// CheckConversionResults Constructor
        /// </summary>
        public CheckConversionResults()
        {
            _init();
        }

        /// <summary>
        /// CheckConversionResults Constructor
        /// </summary>
        /// <param name="targetUnitOfMeasure"></param>
        public CheckConversionResults(string targetUnitOfMeasure)
        {
            TargetUnitOfMeasure = targetUnitOfMeasure;
            _init();
        }

        /// <summary>
        /// Initializes the CheckConversionResults object
        /// </summary>
        private void _init()
        {
            ConvertibleInputUnitOfMeasure = new List<string>();
            NotConvertibleInputUnitOfMeasures = new List<string>();
        }

        /// <summary>
        /// Add convertible input unit of measure
        /// </summary>
        /// <param name="inputUnitOfMeasure"></param>
        public void AddConvertibleInputUnitOfMeasure(string inputUnitOfMeasure)
        {
            if (!ConvertibleInputUnitOfMeasure.Contains(inputUnitOfMeasure))
                ConvertibleInputUnitOfMeasure.Add(inputUnitOfMeasure);
        }

        /// <summary>
        /// Add not convertible input unit of measure
        /// </summary>
        /// <param name="inputUnitOfMeasure"></param>
        public void AddNotConvertibleInputUnitOfMeasure(string inputUnitOfMeasure)
        {
            if (!NotConvertibleInputUnitOfMeasures.Contains(inputUnitOfMeasure))
                NotConvertibleInputUnitOfMeasures.Add(inputUnitOfMeasure);
        }

        /// <summary>
        /// TargetUnitOfMeasure
        /// </summary>
        public string TargetUnitOfMeasure { get; set; }
        /// <summary>
        /// ConvertibleInputUnitOfMeasure
        /// </summary>
        public List<string> ConvertibleInputUnitOfMeasure { get; set; }
        /// <summary>
        /// NotConvertibleInputUnitOfMeasures
        /// </summary>
        public List<string> NotConvertibleInputUnitOfMeasures { get; set; }
    }
}
