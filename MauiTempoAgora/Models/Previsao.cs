using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MauiTempoAgora.Models
{
    public class Previsao
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime data { get => DateTime.Now; }
        public string cidade { get; set; }
        public string temperatura { get; set; }
        public string descricao { get; set; }
    }
}
