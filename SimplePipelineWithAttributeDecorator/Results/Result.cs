namespace SimplePipelineWithAttributeDecorator.Results
{
    public class Result
    {
        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }

        public static explicit operator Result(Exception exception)
        {
            return Failure(Error.Exception);
        }

        public static Result Success()
        {
            return new Result(true, Error.None);
        }

        public static Result Failure(Error error)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(error.Code, "error.Code");
            ArgumentException.ThrowIfNullOrWhiteSpace(error.Message, "error.Message");
            return new Result(false, error);
        }

        public bool IsSuccess
        {
            get;
        }

        public bool IsFailure => !IsSuccess;

        public Error Error
        {
            get;
        }

        internal Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
