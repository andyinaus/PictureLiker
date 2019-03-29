namespace PictureLiker.Exceptioons
{
    public class EmailIsAlreadyInUseException : DomainException
    {
        public EmailIsAlreadyInUseException(string email) 
            : base($"Email '{email}' is already in use.")
        {
            Email = email;
        }

        public string Email { get; private set; }

        public override string ErrorCode => "U-001";
    }
}
