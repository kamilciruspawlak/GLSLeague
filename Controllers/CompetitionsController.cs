using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlsLeague.Models;
using GlsLeague.ViewModel;

namespace GlsLeague.Controllers
{
    public class CompetitionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Competitions
        public ActionResult Index()
        {
            return View(db.Competitions.ToList());
        }

        // GET: Competitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            return View(competition);
        }

        // GET: Competitions/Create
        public ActionResult Create(CompetitionEventsVM viewModel)
        {
           
            
            viewModel.EventsList = db.Events.ToList();
            

            return View(viewModel);
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetitionEventsVM viewModel, int[] test )
        {
            if (ModelState.IsValid)
            {
                db.Competitions.Add(viewModel.Competition);
                db.SaveChanges();

                foreach (var eventId in viewModel.SelectedEventsList)
                {
                    var competitionsEvents = new CompetitionEvents();
                    
                    competitionsEvents.CompetitionID = viewModel.Competition.ID;
                    competitionsEvents.EventID = int.Parse(eventId);
                    db.CompetitionEvents.Add(competitionsEvents);
                    db.SaveChanges();
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Competitions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompetitionID,Name,Description,StartDate,EndDate,IsCompetitionActvie,IsRegistrationOpen")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // GET: Competitions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            return View(competition);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Competition competition = db.Competitions.Find(id);
            db.Competitions.Remove(competition);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
