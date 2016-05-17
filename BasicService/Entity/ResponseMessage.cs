using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicService
{
    public class ResponseMessage
    {
        public string code { get; set; }
        public string msg { get; set; }
    }
    public class ResponseMessage<T> : ResponseMessage
    {
        public T ext { get; set; }
    }
}
