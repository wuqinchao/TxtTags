namespace TxtTags.Common
{
    public class Result<T>
    {
        private int code = int.MinValue;
        public int Code
        {
            get => code;
            set => code = value;
        }

        private string message = string.Empty;
        public string Message
        {
            get => message;
            set => message = value;
        }
        private T data;
        public T Data
        {
            get => data;
            set => data = value;
        }
        /// <summary>
        /// 是否是成功结果
        /// </summary>
        public bool OK
        {
            get { return Code == (int)ErrorCode.SUCC; }
        }

        public Result(int code)
        {
            Code = code;
            if(!OK)
            {
                Message = EnumConvert<ErrorCode>.GetDescription((ErrorCode)code);
            }
        }
        public Result(ErrorCode code)
        {
            Code = (int)code;
            if (!OK)
            {
                Message = EnumConvert<ErrorCode>.GetDescription(code);
            }
        }
        public Result(int code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public Result(ErrorCode code, string message, T data)
        {
            Code = (int)code;
            Message = message;
            Data = data;
        }
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public Result(ErrorCode code, string message)
        {
            Code = (int)code;
            Message = message;
        }
        //public Result(int code, T data)
        //{
        //    Code = code;
        //    Data = data;
        //    if (!OK)
        //    {
        //        Message = EnumConvert<ErrorCode>.GetDescription((ErrorCode)code);
        //    }
        //}
    }

    public enum ErrorCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCC = 0,
        /// <summary>
        /// 未知错误
        /// </summary>
        UNKNOWN = 9999,
    }
}
