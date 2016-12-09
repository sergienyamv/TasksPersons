using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TasksPersons.Storages
{
    public abstract class BaseStorage<T>            //Базовый класс для доступа к БД
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["JIRAConnectionString"].ConnectionString;

        protected abstract string TableName { get; }        //выборка Таблицы по имени

        public ISet<T> GetAll()                     //выборка всех записей таблицы
        {
            return Select(new SqlCommand(String.Format("SELECT * FROM {0}", TableName)));
        }

        public ISet<T> GetByIds(ISet<int> ids)      //поиск записей по Id
        {
            var parameters = new Dictionary<String, int>();
            int i = 1;
            foreach (var id in ids) {
                parameters["@p" + (i++)] = id;
            }
            var command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID IN ({1})", TableName, String.Join(",", parameters.Keys)));
            foreach (var p in parameters) {
                command.Parameters.Add(p.Key, SqlDbType.Int);
                command.Parameters[p.Key].Value = p.Value;
            }
            return Select(command);
        }

        public void DeleteById(int id)              //удаление записи по Id
        {
            var command = new SqlCommand(String.Format("DELETE FROM {0} WHERE ID = @id", TableName));
            command.Parameters.Add("@id", SqlDbType.Int);
            command.Parameters["@id"].Value = id;
            Delete(command);
        }

        public void Add(ISet<T> entities)           //добавление записи в Таблицу
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                connection.Open();
                var adapter = new SqlDataAdapter() {
                    SelectCommand = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID = -1", TableName), connection)
                };

                var dataSet = new DataSet();
                adapter.Fill(dataSet);
                foreach (var entity in entities) {
                    var newRow = dataSet.Tables[0].NewRow();
                    ConvertEntityToRow(newRow, entity);
                    dataSet.Tables[0].Rows.Add(newRow);
                }
                new SqlCommandBuilder(adapter).DataAdapter.Update(dataSet);
            }
        }

        public void Update(ISet<T> entities)        //редактирование записи
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                connection.Open();
                var adapter = new SqlDataAdapter() {
                    SelectCommand = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID IN ({1})", TableName, String.Join(",", entities.Select(GetId))), connection)
                };

                var dataSet = new DataSet();
                adapter.Fill(dataSet);
                foreach (var entity in entities) {
                    dataSet.Tables[0].PrimaryKey = new[] {dataSet.Tables[0].Columns["ID"]};
                    var row = dataSet.Tables[0].Rows.Find(GetId(entity));
                    ConvertEntityToRow(row, entity);
                }
                new SqlCommandBuilder(adapter).DataAdapter.Update(dataSet);
            }
        }

        protected void Delete(SqlCommand command) 
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                connection.Open();

                command.Connection = connection;
                var adapter = new SqlDataAdapter {
                    DeleteCommand = command
                };

                adapter.DeleteCommand.ExecuteNonQuery();
            }
        }

        protected ISet<T> Select(SqlCommand selectCommand) 
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                connection.Open();

                selectCommand.Connection = connection;
                var adapter = new SqlDataAdapter {
                    SelectCommand = selectCommand
                };

                var dataSet = new DataSet();
                adapter.Fill(dataSet, TableName);
                var entities = new HashSet<T>();
                foreach (DataRow row in dataSet.Tables[TableName].Rows) {
                    entities.Add(ConvertRowToEntity(row));
                }
                return entities;
            }
        }

        protected abstract T ConvertRowToEntity(DataRow row);               //для связывания модели и таблицы

        protected abstract void ConvertEntityToRow(DataRow row, T entity);  //для связывания модели и таблицы

        protected abstract int GetId(T entity);                             //Id по записи из Таблицы
    }
}