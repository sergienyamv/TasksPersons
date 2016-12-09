using System.Data;
using System;
using TasksPersons.Models;

namespace TasksPersons.Storages
{
    public class TaskStorage : BaseStorage<Task>        //перегрузка для таблицы Task
    {
        protected override string TableName
        {
            get { return "Task"; }
        }

        protected override Task ConvertRowToEntity(DataRow row)
        {
            return new Task()
            {
                TaskId = row.Field<int>("Id"),
                Name = row.Field<string>("Title"),
                Description = row.Field<string>("Description"),
                State = (TaskState)row.Field<int>("State"),
                BeginDate = row.Field<DateTime>("Start"),
                EndDate = row.Field<DateTime>("End"),
                PersonId = row.Field<int>("PersonId")
            };
        }

        protected override void ConvertEntityToRow(DataRow row, Task entity)
        {
            row["Title"] = entity.Name ?? "";
            row["Description"] = entity.Description ?? "";
            row["State"] = (int)entity.State;
            row["Start"] = entity.BeginDate;
            row["End"] = entity.EndDate;
            row["PersonId"] = entity.PersonId;
        }

        protected override int GetId(Task entity)
        {
            return entity.TaskId;
        }
    }
}