namespace MCInstaller.Core.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException() : base("Error occured during parsing.") { }

        public ParseException(string message) : base(message) { }
    }
}
