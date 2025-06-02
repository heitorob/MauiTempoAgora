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
            conectar.CreateTableAsync<Tempo>().Wait();
        }

        public Task<int> Insert(Tempo p)
        {
            return conectar.InsertAsync(p);
        }

        public Task<int> Delete(int id)
        {
            return conectar.Table<Tempo>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Tempo>> GetAll()
        {
            return conectar.Table<Tempo>().ToListAsync();
        }

        public Task<List<Tempo>> Search(string q)
        {
            string sql = "SELECT * FROM Tempo WHERE cidade LIKE ?";
            return conectar.QueryAsync<Tempo>(sql, "%",  q, "%");
        }
    }
}
