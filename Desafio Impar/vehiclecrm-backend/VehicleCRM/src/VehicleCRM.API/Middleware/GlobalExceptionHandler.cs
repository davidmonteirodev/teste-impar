using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidation;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            case ValidationException validationException:
                problemDetails = HandleValidationException(validationException, httpContext);
                break;

            case DomainException domainException:
                problemDetails = HandleDomainException(domainException, httpContext);
                break;

            case EntityNotFoundException entityNotFoundException:
                problemDetails = HandleEntityNotFoundException(entityNotFoundException, httpContext);
                break;

            default:
                problemDetails = HandleUnexpectedException(exception, httpContext);
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails HandleValidationException(
        ValidationException exception,
        HttpContext httpContext)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        _logger.LogWarning(
            exception,
            "Validação falhou com {ErrorCount} erro(s)",
            errors.Count);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Erro de Validação",
            Detail = "Um ou mais erros de validação ocorreram.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["errors"] = errors;
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        return problemDetails;
    }

    private ProblemDetails HandleDomainException(
        DomainException exception,
        HttpContext httpContext)
    {
        _logger.LogWarning(
            exception,
            "Exceção da camada de domínio: {Message}",
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Erro de Domínio",
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        return problemDetails;
    }

    private ProblemDetails HandleEntityNotFoundException(
        EntityNotFoundException exception,
        HttpContext httpContext)
    {
        _logger.LogWarning(
            exception,
            "Entidade não encontrada: {EntityName} com Id: {EntityId}",
            exception.EntityName,
            exception.EntityId);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Entidade Não Encontrada",
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["entityName"] = exception.EntityName;
        problemDetails.Extensions["entityId"] = exception.EntityId;
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        return problemDetails;
    }

    private ProblemDetails HandleUnexpectedException(
        Exception exception,
        HttpContext httpContext)
    {
        _logger.LogError(
            exception,
            "Um erro inesperado ocorreu: {Message}",
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Erro Interno do Servidor",
            Detail = "Um erro inesperado ocorreu. Por favor, tente novamente mais tarde.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        return problemDetails;
    }
}
