using System;

namespace Avelango.Models.Accessory
{
    public class OperationResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public OperationResult() {
            IsSuccess = true;
        }

        public OperationResult(T data) {
            IsSuccess = true;
            Data = data;
        }

        public OperationResult(Exception exception) {
            IsSuccess = false;
            Exception = exception;
        }
    }
}
