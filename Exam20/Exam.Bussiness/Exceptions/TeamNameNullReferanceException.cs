using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Bussiness.Exceptions
{
    public class TeamNameNullReferanceException : Exception
    {
        public string PropertyName { get; set; }
        public TeamNameNullReferanceException( string propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
