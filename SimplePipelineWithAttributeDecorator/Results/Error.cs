namespace SimplePipelineWithAttributeDecorator.Results
{
    public record Error
    {
        internal static Error None => new Error(Guid.Empty, string.Empty, string.Empty);

        internal static Error Exception => new Error();

        public static Error Create(string code, string message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            return new Error(Guid.NewGuid(), code, message);
        }

        public Guid Id { get; }

        public string Code { get; }

        public string Message { get; }

        private Error(Guid id, string code, string message)
        {
            Id = id;
            Code = code;
            Message = message;
        }
        
        private Error()
        {
            Id = Guid.NewGuid();
            Code = "Exception";
            Message = $"An exception was thrown id : {Id.ToString()} (see log)";
        }
    }
}
