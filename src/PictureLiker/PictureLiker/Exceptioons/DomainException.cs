using System;

namespace PictureLiker.Exceptioons
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string errorMessage)
            : base(errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }
        public abstract string ErrorCode { get; }
    }
}
