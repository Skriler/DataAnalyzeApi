using AutoMapper;
using DataAnalyzeApi.DAL.Repositories.Analysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalyzeApi.Controllers.Analysis;

[ApiController]
[Route("api/cluster-analysis-results")]
[Authorize(Policy = "UserOrAdmin")]
[Produces("application/json")]
public class ClusterAnalysisResultController(
    ClusterAnalysisResultRepository repository,
    IMapper mapper,
    ILogger<ClusterAnalysisResultController> logger
    ) : ControllerBase
{
    private readonly ClusterAnalysisResultRepository repository = repository;
    private readonly IMapper mapper = mapper;
    private readonly ILogger<ClusterAnalysisResultController> logger = logger;
}
