using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTempoAgora.Models;
using SQLite;

namespace MauiTempoAgora.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection conectar;

        public SQLiteDatabaseHelper(string rota)
        {
            conectar = new SQLiteAsyncConnection(rota);
            conectar.CreateTableAsync<Previsao>().Wait();
        }
    }
}
