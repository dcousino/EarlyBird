using Newtonsoft.Json;
using System;

namespace EarlyBird.Data
{
    public interface IFour25Schedule
    {
        public string EventItemId { get; set; }
        public string EventTypeId { get; set; }
    }

    public class Four25MemberSchedule : IFour25Schedule
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int Id { get; set; }

        [JsonProperty("clubNumber")]
        public string ClubNumber { get; set; }

        [JsonProperty("clubName")]
        public string ClubName { get; set; }

        [JsonProperty("clubCity")]
        public string ClubCity { get; set; }

        [JsonProperty("eventItemId")]
        public string EventItemId { get; set; }

        [JsonProperty("eventKind")]
        public string EventKind { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("eventStatusCode")]
        public string EventStatusCode { get; set; }

        [JsonProperty("dayOfWeek")]
        public string DayOfWeek { get; set; }

        [JsonProperty("eventDate")]
        public string EventDate { get; set; }

        [JsonProperty("eventStartTime")]
        public string EventStartTime { get; set; }

        [JsonProperty("eventEndTime")]
        public string EventEndTime { get; set; }

        [JsonProperty("eventDurationName")]
        public string EventDurationName { get; set; }

        [JsonProperty("levelName")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int LevelName { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("employeeId")]
        public string EmployeeId { get; set; }

        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("free")]
        public bool Free { get; set; }

        [JsonProperty("unfunded")]
        public bool Unfunded { get; set; }

        [JsonProperty("allowCancel")]
        public bool AllowCancel { get; set; }

        [JsonProperty("allowCancelCharge")]
        public bool AllowCancelCharge { get; set; }

        [JsonProperty("allowCancelNoCharge")]
        public bool AllowCancelNoCharge { get; set; }

        [JsonProperty("memberVerifyToComplete")]
        public bool MemberVerifyToComplete { get; set; }

        [JsonProperty("isMemberVerified")]
        public bool IsMemberVerified { get; set; }

        [JsonProperty("isLocationRequiredToComplete")]
        public bool IsLocationRequiredToComplete { get; set; }

        [JsonProperty("currentEnrollment")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int CurrentEnrollment { get; set; }

        [JsonProperty("eventComment")]
        public string EventComment { get; set; }

        [JsonProperty("enrolled")]
        public bool Enrolled { get; set; }

        [JsonProperty("allowEnrollFromWaitlist")]
        public bool AllowEnrollFromWaitlist { get; set; }

        public string EventTypeId { get; set; } = Globals.WorkOutEventId;
    }

    public class Four25ClassSchedule : IFour25Schedule
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int Id { get; set; }

        [JsonProperty("eventItemId")]
        public string EventItemId { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("dayOfWeek")]
        public string DayOfWeek { get; set; }

        [JsonProperty("eventDate")]
        public string EventDate { get; set; }

        [JsonProperty("eventStartTime")]
        public string EventStartTime { get; set; }

        [JsonProperty("eventEndTime")]
        public string EventEndTime { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("employeeId")]
        public string EmployeeId { get; set; }

        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("enrolled")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int Enrolled { get; set; }

        [JsonProperty("maxEnrollment")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int MaxEnrollment { get; set; }

        [JsonProperty("eventTypeId")]
        public string EventTypeId { get; set; }

        [JsonProperty("levelId")]
        public string LevelId { get; set; }

        [JsonProperty("allowEnroll")]
        public bool AllowEnroll { get; set; }

        [JsonProperty("isFree")]
        public bool IsFree { get; set; }

        [JsonProperty("waitlistEnabled")]
        public bool WaitlistEnabled { get; set; }

        [JsonProperty("maxWaitlist")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int MaxWaitlist { get; set; }

        [JsonProperty("currentWaitlist")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int CurrentWaitlist { get; set; }

        public string InternalEnrollmentStatus { get; set; }
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(int) || t == typeof(int?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            int l;
            if (int.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type int");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (int)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}