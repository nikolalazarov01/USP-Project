using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Usp_Project.Utils;

public static class OperationResultExtensions
{
    public static bool ValidateNotNull<TValue>(this OperationResult operationResult,
        [NotNullWhen(true)] TValue? value, [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null, [CallerLineNumber] int line = -1,
        [CallerArgumentExpression("value")] string? expression = null)
    {
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));
        if (value is not null) return true;

        var error = $"{FormatErrorMessage(filePath, memberName, line, expression)} should not be null.";
        operationResult.AddError(error);

        return false;
    }

    public static void AppendError(this OperationResult operationResult, Exception exception)
    {
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));
        if (exception is null) return;

        var errorMessage = exception.Message;
        operationResult.AddError(errorMessage);
    }
    
    public static TOperationResult AppendErrors<TOperationResult>(this TOperationResult operationResult, OperationResult other)
        where TOperationResult : OperationResult
    {
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));

        foreach (var error in other.Errors.OrEmptyIfNull().IgnoreNullValues()!)
        {
            operationResult.AddError(error);
        }

        return operationResult;
    }
    
    private static string FormatErrorMessage(string filePath, string memberName, int line, string argumentExpression) => $"{filePath} ({memberName};{line}) - Expression [{argumentExpression}]";
}