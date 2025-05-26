﻿using MauiTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? tempo = null;
            string chave = "6135072afe7f6cec1537d5cb08a5a1a2";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage rspt = await client.GetAsync(url);
                if (rspt.IsSuccessStatusCode)
                {
                    string json = await rspt.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);
                    DateTime time = new();
                    DateTimeOffset sunrise = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"]);
                    DateTimeOffset sunset = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunset"]);

                    tempo = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"]![0]!["description"],
                        main = (string)rascunho["weather"]![0]!["main"],
                        tempmin = (double)rascunho["main"]["temp_min"],
                        tempmax = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.LocalDateTime.ToString(),
                        sunset = sunset.LocalDateTime.ToString(),
                    };
                }
            }
            return tempo;
        }
    }
}
