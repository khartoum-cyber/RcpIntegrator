using System.Text;

namespace RcpIntegrator.Tests
{
    static class CsvHelpers
    {
        public static MemoryStream ToStream(string content)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }
    }
}
