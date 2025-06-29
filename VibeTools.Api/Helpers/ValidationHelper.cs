using FluentValidation.Results;
using VibeTools.Api.Extensions;

namespace VibeTools.Api.Helpers;

public static class ValidationHelper
{
    public static Dictionary<string, string> ConvertValidationResultToCamelCaseDict(ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(e => e.PropertyName.PascalToCamelCase())
            .ToDictionary(
                group => group.Key,
                group => group.First().ErrorMessage
            );
    }
}