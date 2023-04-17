using Microsoft.AspNetCore.Mvc;
using strolls_bot.Models;
using strolls_bot.Database;
using strolls_bot.Calculations;

namespace strolls_bot.Controllers
{
    [ApiController]
    [Route("api/strolls")]
    public class StrollsController : ControllerBase
    {
        private readonly DataContext _context;

        public StrollsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("info")]
        public IActionResult GetInfo([FromBody] Imei imei)
        {
            // Упорядочиваю всех прогулки по времени
            List<TrackLocation> tracks = _context.TrackLocations
                .Where(e => e.Imei == imei.Value)
                .OrderBy(e => e.DateTrack)
                .ToList();

            List<TrackLocation> strolls = new List<TrackLocation>(tracks);

            // Для каждой уникальной прогулки добавляю информацию о её длительности по времени и километражу
            for (int i = 0; i < strolls.Count - 1;)
            {
                DateTime firstTrack = strolls[i].DateTrack;
                DateTime nextTrack = strolls[i + 1].DateTrack;
                TimeSpan timeDifference = nextTrack - firstTrack;

                double distanceInMeters = GeoUtils.Distance((double)strolls[i].Latitude, (double)strolls[i].Longitude, (double)strolls[i + 1].Latitude, (double)strolls[i + 1].Longitude);
                if (timeDifference.TotalMinutes < 30)
                {
                    strolls[i].TotalTime += (double)timeDifference.TotalMinutes;
                    strolls[i].TotalDistance += (double)distanceInMeters;
                    strolls.RemoveAt(i + 1);
                }
                else
                {
                    i++;
                }
            }

            // Записываю основные данные: сколько было прогулок, их общее время и километраж

            int totalCount = strolls.Count;
            double totalTime = 0;
            double totalDistance = 0;

            foreach (var stroll in strolls)
            {
                totalTime += stroll.TotalTime;
                totalDistance += stroll.TotalDistance;

            }

            MainData data = new MainData()
            {
                TotalStrolls = totalCount,
                TotalTime = totalTime,
                TotalMeters = totalDistance
            };


            // Проверяю сколько прогулок было за день, их итоговый километраж и время
            int totalDayCount = 0;
            double totalDayTime = 0;
            double totalDayDistance = 0;
            DateTime? thisDay = null;

            foreach (var stroll in strolls)
            {
                if (thisDay == null)
                {
                    thisDay = stroll.DateTrack;
                }
                if (stroll.DateTrack == thisDay.Value)
                {
                    totalDayTime += stroll.TotalTime;
                    totalDayDistance += stroll.TotalDistance;
                    totalDayCount += 1;
                }
            }

            DayStrollsInfo dayStrolls = new DayStrollsInfo()
            {
                TotalDayStrollsCount = totalDayCount,
                TotalDayTime = totalDayTime,
                TotalDayDistance = totalDayDistance

            };

            // Топ-10 прогулок по заданному IMEI
            List<TrackLocation> topStrolls = new List<TrackLocation>(strolls).OrderBy(p => p.TotalDistance).Take(10).ToList();

            Answer answer = new Answer()
            {
                Strolls = strolls,
                StrollsData = data,
                DayInfo = dayStrolls,
                TopStrolls = topStrolls,
            };

            var strollsCount = tracks.Count;

            return Ok(answer);
        }
    }
}
