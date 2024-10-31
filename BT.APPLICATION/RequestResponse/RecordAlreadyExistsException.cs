using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.APPLICATION.RequestResponse
{
    public class RecordAlreadyExistsException : Exception
    {
        public RecordAlreadyExistsException(string message) : base(message) { }
    }
}