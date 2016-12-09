using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TasksPersons.Models;
using TasksPersons.Storages;

namespace TasksPersons.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskStorage _storage = new TaskStorage();
        private readonly PersonStorage _storageP = new PersonStorage();

        // GET: Task
        public ActionResult Index()         //список Задач
        {
            return View(_storage.GetAll());
        }

        // GET: Default/Create
        public ActionResult Create()        //создание новой Задачи
        {
            ViewBag.PersonId = new SelectList(_storageP.GetAll(), "PersonId", "FullName");
            return View();
        }

        // POST: Default/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskID,Name,Description,BeginDate,EndDate,State,PersonId")] Task newTask)
        {
            if (ModelState.IsValid)
            {
                _storage.Add(new HashSet<Task>() { newTask });

                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(_storageP.GetAll(), "PersonId", "FullName");
            return View();
        }

        // GET: Default/Edit/5
        public ActionResult Edit(int id)        //редактирование Задачи
        {
            Task editTask = _storage.GetAll().First(m => m.PersonId == id);
            ViewBag.PersonId = new SelectList(_storageP.GetAll(), "PersonId", "FullName", editTask.PersonId);
            return View(editTask);
        }

        // POST: Default/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskID,Name,Description,BeginDate,EndDate,State,PersonId")] Task editTask)
        {           
            if (ModelState.IsValid)
            {
                _storage.Update(new HashSet<Task>() { editTask });
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(_storageP.GetAll(), "PersonId", "FullName", editTask.PersonId);
            return View(editTask);

        }

        // GET: Default/Delete/5
        public ActionResult Delete(int id)      //удаление Задачи
        {
            Task deleteTask = _storage.GetAll().First(m => m.PersonId == id);
            return View(deleteTask);
        }

        // POST: Default/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var deletePerson = _storage.GetAll().First(m => m.PersonId == id);
            return RedirectToAction("Index");

        }
    }


    /*public static class AbsTasks         //класс заглушка
    {
        public static List<Task> lt = new List<Task>() {
            new Task(){ TaskId = 0, Name = "Develop", Description = "Develop develop", BeginDate = DateTime.Today, EndDate = DateTime.Today.AddDays(10), State = TaskState.Plan, PersonId = 0 },
            new Task(){ TaskId = 1, Name = "Design", Description = "Design design", BeginDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(5), State = TaskState.Process, PersonId = 1 },
            new Task(){ TaskId = 2, Name = "Coffee", Description = "Coffee time", BeginDate = DateTime.Today.AddDays(-10), EndDate = DateTime.Today, State = TaskState.Completed, PersonId = 1 }
        };
    }
    public static class AbsPersons       //класс заглушка
    {
        public static List<Person> lp = new List<Person>() {
            new Person() { PersonId = 0, Name = "Ivan", Surname = "Popov", MiddleName = "Vladis"},
            new Person() { PersonId = 1, Name = "Nikolay", Surname = "Ferchuk", MiddleName = "Levishin"},
            new Person() { PersonId = 2, Name = "Anna", Surname = "Knyaz", MiddleName = "Mihailov"}
        };
    }*/
}