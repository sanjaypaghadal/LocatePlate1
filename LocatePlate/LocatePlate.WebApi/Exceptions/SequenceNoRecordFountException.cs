using System;

namespace LocatePlate.WebApi.Exceptions
{
    public class SequenceNoRecordFountException : Exception, ISequenceNoRecordFountException
    {
        public SequenceNoRecordFountException()
        {

        }
        public SequenceNoRecordFountException(string message) : base(message)
        {

        }
    }
}
