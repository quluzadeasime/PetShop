using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions
{
    public class MemberNullException : Exception
    {
        public string PropertyName { get; set; }
        public MemberNullException(string propName,string? message) : base(message)
        {
            PropertyName = propName;
        }
    }
}
