using EarlyBird.Contexts;
using EarlyBird.Data;
using EarlyBird.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EarlyBird.Services
{
    public enum ScheduleType
    {
        Member,
        Class
    }

    public class Four25Service
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ScheduleDbContext _context;
        private readonly Settings _settings;

        public Four25Service(IHttpClientFactory httpClient, ScheduleDbContext context, IConfiguration configuration, IOptions<Settings> options)
        {
            _config = configuration;
            _httpClientFactory = httpClient;
            _settings = options.Value;
            _context = context;
        }

        public async Task Login(string userName)
        {
            var client = _httpClientFactory.CreateClient(Globals.Four25Client);

            var formContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("spring-security-redirect", "#activities/classes"),
                new KeyValuePair<string, string>("username", userName),
                                new KeyValuePair<string, string>("password", _config[$"425-{userName}-password"])
            });

            var res = await client.PostAsync("performLogin", formContent);
        }

        public IEnumerable<string> GetUsers()
        {
            return _settings.PassWords.Keys;
        }

        public async Task AddReservation(Four25ClassSchedule schedule, string userName)
        {
            var res = new Reservation()
            {
                UserName = userName,
                ClubId = _settings.ClubId,
                EventId = schedule.EventItemId,
                StartTime = DateTime.ParseExact($"{schedule.EventDate} {schedule.EventStartTime}", Globals.GymTimeFormat, CultureInfo.InvariantCulture)
            };
            await _context.Reservations.AddAsync(res);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Four25ClassSchedule>> GetScheduleAsync(string userName, DateTime lowDate, DateTime highDate)
        {
            string memberUrl = GetScheduleUrl(ScheduleType.Member, lowDate, highDate);
            string classUrl = GetScheduleUrl(ScheduleType.Class, lowDate, highDate);
            var m = await GetFour25Schedules<Four25MemberSchedule>(memberUrl);
            var c = await GetFour25Schedules<Four25ClassSchedule>(classUrl);

            foreach (var entry in c)
            {
                if (m.Any(x => x.EventItemId == entry.EventItemId && x.Enrolled))
                {
                    entry.InternalEnrollmentStatus = Globals.Enrolled;
                }
                else if (_context.Reservations.Any(x => x.EventId == entry.EventItemId && x.UserName == userName))
                {
                    entry.InternalEnrollmentStatus = Globals.Pending;
                }
            }

            return c;
        }

        private async Task<IEnumerable<T>> GetFour25Schedules<T>(string url) where T : class, IFour25Schedule
        {
            var client = _httpClientFactory.CreateClient(Globals.Four25Client);

            var res = await client.GetAsync(url);

            res.EnsureSuccessStatusCode();
            var t = await res.Content.ReadAsStringAsync();
            try
            {
                var sch = JsonConvert.DeserializeObject<T[]>(t).Where(s => s.EventTypeId == _settings.WorkOutEventId);
                return sch;
            }
            catch (Exception e)
            {
                var a = e;
                throw;
            }
        }

        private string GetScheduleUrl(ScheduleType scheduleType, DateTime lowDate, DateTime highDate)
        {
            var schType = scheduleType == ScheduleType.Member ? Globals.MemberSchedule : Globals.ClassSchedule;
            return $"scheduling/{schType}?club={_settings.ClubId}&lowDate={lowDate.ToShortDateString()}&highDate={highDate.ToShortDateString()}&_={DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
        }
    }
}