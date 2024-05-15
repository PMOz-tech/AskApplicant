using Newtonsoft.Json;
using System.Diagnostics;

namespace AskApplicant.Core.Models.Responses
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }

        public string Message { get; set; }


        public T Data { get; set; }

        public BaseResponse()
            : this(status: true, string.Empty)
        {
        }

        public BaseResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public BaseResponse(bool status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
