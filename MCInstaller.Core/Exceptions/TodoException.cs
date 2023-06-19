namespace MCInstaller.Core.Exceptions
{
    public class TodoException : Exception
    {
        public TodoException() : this("Feature to be implemented") { }

        public TodoException(string message) : base(message) { }
    }
}
