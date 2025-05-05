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

        public Task<int> Insert(Previsao p)
        {
            return conectar.InsertAsync(p);
        }

        public Task<int> Delete(int id)
        {
            return conectar.Table<Previsao>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Previsao>> GetAll()
        {
            return conectar.Table<Previsao>().ToListAsync();
        }

        public Task<List<Previsao>> SearchByDate(DateTime q)
        {
            string sql = "SELECT * FROM Previsao WHERE data LIKE ?";
            return conectar.QueryAsync<Previsao>(sql, q);
        }

        public Task<List<Previsao>> SearchByLocation(string q)
        {
            string sql = "SELECT * FROM Previsao WHERE cidade LIKE ?";
            return conectar.QueryAsync<Previsao>(sql, "%",  q, "%");
        }
    }
}
