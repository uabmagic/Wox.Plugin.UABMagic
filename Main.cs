using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Wox.Plugin.UABMagic
{
    public class Main : IPlugin
    {
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            var currentSongInfo = GetCurrentSongInfo();

            return new List<Result>
            {
                new Result
                {
                    IcoPath = $"Images\\logo.png",
                    SubTitle = $"{currentSongInfo.Artist}",
                    Title = $"{currentSongInfo.Title}",
                    Action = _ => { return true; }
                }
            };
        }

        private CurrentTrack GetCurrentSongInfo()
        {
            using (var httpClient = new HttpClient())
            {
                var response = Task.Run(async () => await httpClient.GetStringAsync("https://uabmagic-api.vercel.app/api/songs/nowplaying")).Result;
                var jsonObject = JObject.Parse(response);

                return new CurrentTrack
                {
                    Artist = (string) jsonObject["themeParkAndLand"],
                    Title = (string) jsonObject["attractionAndSong"]
                };
            }
        }
    }

    public class CurrentTrack
    {
        public string Artist { get; set; }
        public string Title { get; set; }
    }
}
