namespace Railway.EventConsumer.CrossCutting.HandleErrors
{
    public class OperationResult<T>
    {
        public OperationResult() 
        {
            Errors = [];    
        }

        public OperationResult(T value)
        {
            Result = value;
            Errors = [];
        }

        public T? Result { get; private set; }

        public List<Error>? Errors { get; }

        public bool HasErrors() => Errors != null && Errors.Count != 0;
    }
}
