namespace RcpIntegrator.App.Utilities
{
    public static class CsvReader
    {
        public static IEnumerable<string[]> ReadSemicolonSeparated(Stream stream)
        {
            using var reader = new StreamReader(stream);
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                //CsvReader
                var parts = line.Split(';');

                yield return parts;
            }
        }
    }
}