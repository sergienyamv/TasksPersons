using System.Data;
using TasksPersons.Models;

namespace TasksPersons.Storages
{
    public class PersonStorage : BaseStorage<Person>        //перегрузка для таблицы Person
    {
        protected override string TableName {
            get { return "Person"; }
        }

        protected override Person ConvertRowToEntity(DataRow row) {
            return new Person() {
                PersonId = row.Field<int>("Id"),
                Name = row.Field<string>("Name"),
                Surname = row.Field<string>("Surname"),
                MiddleName = row.Field<string>("Middlename")
            };
        }

        protected override void ConvertEntityToRow(DataRow row, Person entity) {
            row["Name"] = entity.Name ?? "";
            row["Surname"] = entity.Surname ?? "";
            row["Middlename"] = entity.MiddleName ?? "";
        }

        protected override int GetId(Person entity) {
            return entity.PersonId;
        }
    }
}