using System;

namespace WebApplication2.Models
{
    public class QualityControl
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public DateTime AnalysisDate { get; set; } = DateTime.Now;
        public string SampleType { get; set; } = string.Empty; // сырье, готовая продукция, промежуточный
        public string ParameterName { get; set; } = string.Empty;
        public decimal? MeasuredValue { get; set; }
        public string StandardValue { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty; // pass, fail
        public string Decision { get; set; } = string.Empty; // approved, blocked, pending
        public string AnalystComment { get; set; } = string.Empty;
        public int? AnalystId { get; set; }
    }
}