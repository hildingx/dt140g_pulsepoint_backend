namespace PulsePoint.Models.DTOs
{
    /// <summary>
    /// DTO för aggregerad statistik per dag för en arbetsplats.
    /// Används av managers vid GET från /api/healthentries/stats/daily.
    /// </summary>
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
