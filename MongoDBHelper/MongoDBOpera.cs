using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDBHelper
{
    public class MongoDBOpera<T> : IDisposable
    { 
        private MongoClient _client;

        private string _dbName;

        private IMongoDatabase _db;

        private IMongoCollection<T> _collection;


        public MongoDBOpera(string connStr, string dbName)
        {
            _client = new MongoClient(connStr);
            _dbName = dbName;
        }

        public void InsertOne(T t)
        {
            try
            {
                GetDataBase();
                GetCollection();
                _collection.InsertOne(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                CloseDb();
            }
        }

        public void InsertMany(List<T> list)
        {
            try
            {
                GetDataBase();
                GetCollection();
                _collection.InsertMany(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDb();
            }
        }

        private void GetDataBase()
        {
            try
            {
                _db = _client.GetDatabase(_dbName);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void GetCollection()
        {
            try
            {
                _collection = _db.GetCollection<T>(typeof(T).Name); 
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void CloseDb()
        {
            try
            {
                _collection = null;
                _db = null;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void DisposeDb()
        {
            try
            {
                CloseDb();

                _collection = null;
                _db = null;
                _client = null;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            DisposeDb();
        }
    }
}
