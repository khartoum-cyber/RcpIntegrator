namespace RcpIntegrator.App.Model
{
    public class WorkDay
    {
        public string Company { get; }
        public string EmployeeCode { get; }
        public DateTime Date { get; }
        public TimeSpan EntryTime { get; }
        public TimeSpan ExitTime { get; }

        public WorkDay(string company, string employeeCode, DateTime date, TimeSpan entry, TimeSpan exit)
        {
            Company = company ?? throw new ArgumentNullException(nameof(company));
            EmployeeCode = employeeCode ?? throw new ArgumentNullException(nameof(employeeCode));
            Date = date;
            EntryTime = entry;
            ExitTime = exit;
        }

        public DateTime Start => Date.Date + EntryTime;

        public DateTime End => (ExitTime >= EntryTime)
            ? Date.Date + ExitTime
            : Date.Date.AddDays(1) + ExitTime;

        public TimeSpan Duration => End - Start;


        public override string ToString()
        {
            var sameDay = End.Date == Start.Date;

            var endDisplay = sameDay
                ? ExitTime.ToString()
                : $"{End:yyyy-MM-dd} {ExitTime}";

            return $"{Company} - {EmployeeCode}  {Date:yyyy-MM-dd}  {EntryTime} - {endDisplay} ({Duration})";
        }
    }
}
