using DataAnalyzeAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataAnalyzeAPI.Middlewares;

/// <summary>
/// Exception filter for handling business logic errors.
/// Logs the error and creates ProblemDetails with additional information for the response to the client.
/// </summary>
public class DataAnalyzeExceptionFilter : IExceptionFilter
{
    private readonly ILogger<DataAnalyzeExceptionFilter> logger;
    private readonly IHostEnvironment environment;

    public DataAnalyzeExceptionFilter(
        ILogger<DataAnalyzeExceptionFilter> logger,
        IHostEnvironment environment)
    {
        this.logger = logger;
        this.environment = environment;
    }

    /// <summary>
    /// Handles exceptions of type DataAnalyzeException.
    /// Logs the error information and prepares a response with problem details.
    /// </summary>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not DataAnalyzeException ex)
            return;

        logger.LogWarning(
            ex,
            "Business exception occurred: {ErrorTitle} - {Message}",
            ex.ErrorTitle,
            ex.Message);

        var problemDetails = new ProblemDetails
        {
            Title = ex.ErrorTitle,
            Status = ex.StatusCode,
            Detail = ex.Message,
            Instance = context.HttpContext.Request.Path,
        };

        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        if (environment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = ex.StackTrace;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = ex.StatusCode
        };

        context.ExceptionHandled = true;
    }
}

public static partial class ErrorHandlingExtensions
{
    /// <summary>
    /// Extension method for registering the DataAnalyzeExceptionFilter.
    /// </summary>
    public static IMvcBuilder AddDataAnalyzeExceptionFilters(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options => options.Filters.Add<DataAnalyzeExceptionFilter>());

        return builder;
    }
}
