namespace BT.SERVICES.DateTimeService
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;

        public double TSpanToHours(double hours)
        {
            var hrs = TimeSpan.FromHours(hours).Hours;
            var tsmin = Math.Round(hours - Math.Truncate(hours), 2,
                MidpointRounding.AwayFromZero);
            var hoursWithTsMin = hrs + tsmin;
            return hoursWithTsMin;
        }
        public double TSpanToMinutes(double tspanMinutes)
            => TimeSpan.FromMinutes(tspanMinutes).Minutes;
        public double TMinutesToHoursAndMinutes(double tspanMinutes)
        {
            var hrs = TimeSpan.FromMinutes(tspanMinutes).Hours;
            var min = TimeSpan.FromMinutes(tspanMinutes).Minutes;
            var time = Convert.ToDouble($"{hrs}.{min}");
            return time;
        }
        public double THoursToHoursAndMinutes(double hours)
        {
            var hrs = TimeSpan.FromHours(hours).Hours;
            var min = TimeSpan.FromHours(hours).Minutes;
            var time = Convert.ToDouble($"{hrs}.{min}");
            return time;
        }
    }

    public interface IDateTimeService
    {
        DateTime Now();
        DateTime UtcNow();
        double TSpanToHours(double hours);
        double TSpanToMinutes(double tspanMinutes);
        double TMinutesToHoursAndMinutes(double tspanMinutes);
        double THoursToHoursAndMinutes(double hours);
    }
}
