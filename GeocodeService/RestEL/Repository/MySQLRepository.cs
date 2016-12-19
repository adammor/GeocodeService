using Dapper;
using MySql.Data.MySqlClient;
using RestEL.Interfaces;
using RestEL.Models;
using RestEL.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RestEL.Repositories
{
    [RegisterRepository(ClassName = "MySQLRepository", ProviderName = "MySql.Data.MySqlClient")]
    public class MySQLRepository<T> : IRepository<T> where T : class
    {
        public MySQLRepository(List<Requestquota> APILimits, Sourcelist SourceDB, Targetlist TargetDB) 
        {
            ResultLimit = APILimits.ToList();

            ConnSourceDB = SourceDB.connectionString;
            CommSelect = SourceDB.getQuery;

            ConnTargetDB = TargetDB.connectionString;
            CommUpdate = TargetDB.updateQuery;

            Get();
        }

        public List<T> RecordSet { get; private set; }
        public string ConnSourceDB { get; private set; }
        public string ConnTargetDB { get; private set; }
        public string CommSelect { get; private set; }
        public string CommUpdate { get; private set; }
        public List<Requestquota> ResultLimit { get; private set; }

        public void Get()
        {
            RecordSet = Retrieve(ConnSourceDB, CommSelect, ResultLimit);   
        }


        private List<T> Retrieve(string connSourceDB, string commSelect, List<Requestquota> resultLimit) 
        {
            List<T> _records = new List<T>();
            using (IDbConnection _conn = new MySqlConnection(connSourceDB))
            {
                try
                {
                    _conn.Open();

                    commSelect = string.Concat(commSelect, " LIMIT @maxFetchCount;");

                    using (var _result = _conn.QueryMultiple(commSelect, new { maxFetchCount = resultLimit[0].day }))
                    {
                        _records = _result.Read<T>().ToList();
                    }  
                }
                catch (MySqlException se)
                {
                    var msg = "SQL error: " + se.Message;
                }
                catch (Exception se)
                {
                    var msg = "Error: " + se.Message;
                }
                finally
                {
                    _conn.Close();
                }
            }
            return _records;
        }

        public void Save(params object[] parameters)
        {
            Update(ConnTargetDB, CommUpdate, parameters);
        }

        private void Update(string connTargetDB, string commUpdate, params object[] parameters)
        {
            using (IDbConnection _conn = new MySqlConnection(connTargetDB))
            {
                try
                {
                    _conn.Open();
                    using ( _conn.QueryMultiple(commUpdate, parameters[0]) ) { }  
                }
                catch (MySqlException se)
                {
                    var msg = "SQL error: " + se.Message;
                }
                catch (Exception se)
                {
                    var msg = "Error: " + se.Message;
                }
                finally
                {
                    _conn.Close();
                }
            }
        }
    }


}
