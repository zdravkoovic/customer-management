using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Customer.API.Extensions;

public static class ProblemDetailsExtensions
{
    public static ProblemDetails CreateNotFound(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status404NotFound,
            httpContext,
            details,
            errors
        );
    }

    public static ProblemDetails CreateBadRequest(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status400BadRequest,
            httpContext,
            details,
            errors
        );
    }

    public static ProblemDetails CreateConflict(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status409Conflict,
            httpContext,
            details,
            errors
        );
    }

    public static ProblemDetails CreateValidation(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status422UnprocessableEntity,
            httpContext,
            details,
            errors
        );
    }

    public static ProblemDetails CreateInternalServerError(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status500InternalServerError,
            httpContext,
            details,
            errors
        );
    }

    public static ProblemDetails CreateUnauthorized(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status401Unauthorized,
            httpContext,
            details,
            errors
        );
    }

    public static ProblemDetails CreateUnexpectedResponse(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        string? details = null,
        IEnumerable<string>? errors = null
    )
    {
        return CreateProblemDetailsWith(
            detailsFactory,
            StatusCodes.Status500InternalServerError,
            httpContext,
            details,
            errors
        );
    }

    private static ProblemDetails CreateProblemDetailsWith(
        ProblemDetailsFactory detailsFactory,
        int statusCode,
        HttpContext httpContext,
        string? message = null,
        IEnumerable<string>? errors = null
    )
    {
        if(errors != null && errors.Any())
        {
            StringBuilder errorList = new();
            errorList.AppendJoin(",", errors);

            return detailsFactory.CreateProblemDetails(
                httpContext,
                statusCode,
                detail: errorList.ToString()
            );
        }
        else
        {
            return detailsFactory.CreateProblemDetails(
                httpContext,
                statusCode,
                detail: message
            );
        }
    }
}