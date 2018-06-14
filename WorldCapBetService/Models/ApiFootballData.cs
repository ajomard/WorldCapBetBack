// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using WorldCapBetService.Models;
//
//    var apiFootballDataModel = ApiFootballDataModel.FromJson(jsonString);

namespace WorldCapBetService.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ApiFootballData
    {
        [JsonProperty("_links")]
        public ApiFootballDataModelLinks Links { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("fixtures")]
        public Fixture[] Fixtures { get; set; }
    }

    public partial class Fixture
    {
        [JsonProperty("_links")]
        public FixtureLinks Links { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("matchday")]
        public long Matchday { get; set; }

        [JsonProperty("homeTeamName")]
        public string HomeTeamName { get; set; }

        [JsonProperty("awayTeamName")]
        public string AwayTeamName { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("odds")]
        public object Odds { get; set; }
    }

    public partial class FixtureLinks
    {
        [JsonProperty("self")]
        public Competition Self { get; set; }

        [JsonProperty("competition")]
        public Competition Competition { get; set; }

        [JsonProperty("homeTeam")]
        public Competition HomeTeam { get; set; }

        [JsonProperty("awayTeam")]
        public Competition AwayTeam { get; set; }
    }

    public partial class Competition
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("goalsHomeTeam")]
        public int? GoalsHomeTeam { get; set; }

        [JsonProperty("goalsAwayTeam")]
        public int? GoalsAwayTeam { get; set; }
    }

    public partial class ApiFootballDataModelLinks
    {
        [JsonProperty("self")]
        public Competition Self { get; set; }

        [JsonProperty("competition")]
        public Competition Competition { get; set; }
    }

    public enum Status { InPlay, Scheduled, Timed, Finished, Canceled, Postponed };

    public partial class ApiFootballData
    {
        public static ApiFootballData FromJson(string json) => JsonConvert.DeserializeObject<ApiFootballData>(json, WorldCapBetService.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ApiFootballData self) => JsonConvert.SerializeObject(self, WorldCapBetService.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new StatusConverter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class StatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "SCHEDULED":
                    return Status.Scheduled;
                case "TIMED":
                    return Status.Timed;
                case "CANCELED":
                    return Status.Canceled;
                case "IN_PLAY":
                    return Status.InPlay;
                case "POSTPONED":
                    return Status.Postponed;
                case "FINISHED":
                    return Status.Finished;
            }
            throw new Exception("Cannot unmarshal type Status");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Status)untypedValue;
            switch (value)
            {
                case Status.Scheduled:
                    serializer.Serialize(writer, "SCHEDULED"); return;
                case Status.Timed:
                    serializer.Serialize(writer, "TIMED"); return;
                case Status.Canceled:
                    serializer.Serialize(writer, "CANCELED"); return;
                case Status.InPlay:
                    serializer.Serialize(writer, "IN_PLAY"); return;
                case Status.Postponed:
                    serializer.Serialize(writer, "POSTPONED"); return;
                case Status.Finished:
                    serializer.Serialize(writer, "FINISHED"); return;
            }
            throw new Exception("Cannot marshal type Status");
        }
    }
}
