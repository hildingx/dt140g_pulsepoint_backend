namespace PulsePoint.Models.DTOs
{
    public class DailyWorkplaceStatsDto
    {
        public DateOnly Date { get; set; }
        public double AverageMood { get; set; }
        public double AverageSleep { get; set; }
        public double AverageStress { get; set; }
        public double AverageActivity { get; set; }
        public double AverageNutrition { get; set; }
        public int EntryCount { get; set; }
    }
}
