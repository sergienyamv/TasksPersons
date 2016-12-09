using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TasksPersons.Models;
using TasksPersons.Storages;

namespace TasksPersons.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonStorage _storage = new PersonStorage();

        public ActionResult Index()         //список Исполнителей
        {
            return View(_storage.GetAll());
        }

        // GET: Default/Create
        public ActionResult Create()        //создание нового Исполнителя      
        {
            return View();
        }

        // POST: Default/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,Name,Surname,MiddleName")] Person newPerson)
        {
            if (ModelState.IsValid)
            {                
                _storage.Add(new HashSet<Person>() { newPerson });

                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Default/Edit/5
        public ActionResult Edit(int id)        //редактирование Исполнителя
        {
            Person editPerson = _storage.GetAll().First(m => m.PersonId == id);
            return View(editPerson);
        }

        // POST: Default/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,Name,Surname,MiddleName")] Person editPerson)
        {
            if (ModelState.IsValid)
            {
                _storage.Update(new HashSet<Person>() { editPerson });
                return RedirectToAction("Index");
            }
            return View(editPerson);

        }

        // GET: Default/Delete/5
        public ActionResult Delete(int id)          //удаление Исполнителя
        {
            var deletePerson = _storage.GetAll().First(m => m.PersonId == id);
            return View(deletePerson);
        }

        // POST: Default/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var deletePerson = _storage.GetAll().First(m => m.PersonId == id);
            //TODO: Проверка, есть ли у исполнителя задачи
           _storage.DeleteById(id);

            return RedirectToAction("Index");

        }
    }
}