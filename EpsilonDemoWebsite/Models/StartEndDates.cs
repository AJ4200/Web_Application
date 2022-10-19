namespace EpsilonDemoWebsite.Models
{
    public class StartEndDates
    {

        public string? startDate { get; set; }
        public string? endDate { get; set; }


        public StartEndDates()
        {
        }

        public StartEndDates(string? startDate, string? endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }


}
