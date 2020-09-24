using ApiWrapper.Abstractions;

namespace ApiWrapper
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string log)
        {
            System.Diagnostics.Debug.WriteLine(log);
        }
    }
}