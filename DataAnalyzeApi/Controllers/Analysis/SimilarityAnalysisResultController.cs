using AutoMapper;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers.Analysis;

[ApiController]
[Route("api/similarity-analysis-results")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class SimilarityAnalysisResultController(
    SimilarityAnalysisResultRepository repository,
    IMapper mapper,
    ILogger<SimilarityAnalysisResultController> logger
    ) : ControllerBase
{
    private readonly SimilarityAnalysisResultRepository repository = repository;
    private readonly IMapper mapper = mapper;
    private readonly ILogger<SimilarityAnalysisResultController> logger = logger;
}
