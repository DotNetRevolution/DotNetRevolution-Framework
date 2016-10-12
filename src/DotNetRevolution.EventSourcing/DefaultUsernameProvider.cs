namespace DotNetRevolution.EventSourcing
{
    public class DefaultUsernameProvider : IUsernameProvider
    {
        private readonly string _username;

        public DefaultUsernameProvider(string username)
        {
            _username = username;
        }

        public string GetUsername()
        {
            return _username;
        }
    }
}
