namespace Usp_Project.Utils;

public class OperationResult
{
    private List<string> _errors = new();
    public IReadOnlyCollection<string> Errors => this._errors.AsReadOnly();

    public bool IsSuccessfull => !this._errors.Any();

    public bool AddError(string errorMessage)
    {
        if (errorMessage is null) return false;
        
        this._errors.Add(errorMessage);
        return true;
    }

    public override string ToString() => string.Join(Environment.NewLine, this._errors);
}

public class OperationResult<T> : OperationResult
{
    public T Data { get; set; }

    public static OperationResult<T> Success(T data)
        => new() { Data = data };
    
}