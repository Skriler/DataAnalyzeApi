using DataAnalyzeAPI.Models.DTOs.Analyse.Similarity;
using DataAnalyzeAPI.Models.Entities;

namespace DataAnalyzeAPI.Services.Analyse;

public class SimilarityComparer
{
    public SimilarityResult CalculateSimilarity(
        Dataset dataset,
        double maxNumericTolerance)
    {
        var similarityResult = new SimilarityResult()
        {
            DatasetId = dataset.Id,
        };

        similarityResult.Similarities = CompareObjects(dataset.Objects);

        return similarityResult;
    }

    private List<SimilarityPairDto> CompareObjects(List<DataObject> objects)
    {
        var similarities = new List<SimilarityPairDto>();

        for (int i = 0; i < objects.Count; ++i)
        {
            var objectA = objects[i];

            for (int j = i + 1; j < objects.Count; ++j)
            {
                var objectB = objects[j];

                var similarity = new SimilarityPairDto()
                {
                    ObjectAId = objectA.Id,
                    ObjectAName = objectA.Name,
                    ObjectBId = objectB.Id,
                    ObjectBName = objectB.Name,
                    SimilarityPercentage = GetSimilarityPercentage(objectA, objectB),
                };

                similarities.Add(similarity);
            }
        }

        return similarities;
    }

    private float GetSimilarityPercentage(DataObject objectA, DataObject objectB)
    {
        if (objectA.Values.Count != objectB.Values.Count)
            throw new InvalidOperationException("Objects must have the same number of parameters");

        return 0;
    }
}
