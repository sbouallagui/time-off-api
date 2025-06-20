namespace Time.Off.Application.Common
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public T? Value { get; private set; }

        private OperationResult() { }

        public static OperationResult<T> Success(T value)
            => new()
            { IsSuccess = true, Value = value };

        public static OperationResult<T> Failure(string errorMessage)
            => new()
            { IsSuccess = false, ErrorMessage = errorMessage };
    }


}
