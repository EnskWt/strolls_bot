using strolls_bot.Database;

namespace strolls_bot.Models
{
    public class Answer
    {
        public List<TrackLocation> Strolls { get; set; }
        public MainData StrollsData { get; set; }
        public DayStrollsInfo DayInfo { get; set; }
        public List<TrackLocation> TopStrolls { get; set; }
    }
}
