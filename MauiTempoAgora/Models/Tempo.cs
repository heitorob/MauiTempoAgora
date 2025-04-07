namespace MauiTempoAgora.Models
{
    public class Tempo
    {
        public double? lon { get; set; }
        public double? lat { get; set; }
        public double? tempmin {  get; set; }
        public double? tempmax { get; set; }
        public int? visibility { get; set; }
        public double? speed { get; set; }
        public string? main {  get; set; }
        public string? description { get; set; }
        public string? sunrise {  get; set; }
        public string? sunset { get; set; }
    }
}
