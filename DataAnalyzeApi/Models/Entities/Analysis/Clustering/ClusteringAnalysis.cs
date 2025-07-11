using DataAnalyzeApi.Models.Enums;

namespace DataAnalyzeApi.Models.Entities.Analysis.Clustering;

public class ClusteringAnalysis : BaseAnalysis
{
    public override AnalysisType AnalysisType => AnalysisType.Clustering;

    public ClusterAlgorithm Algorithm { get; set; }

    public string AlgorithmSettingsJson { get; set; } = string.Empty;

    public NumericDistanceMetricType NumericMetric { get; set; }
    public CategoricalDistanceMetricType CategoricalMetric { get; set; }

    public List<ClusterGroup> Clusters { get; set; } = [];
}
