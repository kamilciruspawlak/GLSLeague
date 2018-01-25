using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlsLeague.Models;

namespace GlsLeague.Controllers
{
    public class CompetitionEventDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompetitionEventDetails
        public ActionResult Index()
        {
            var competitionEventDetails = db.CompetitionEventDetails.Include(c => c.Competition).Include(c => c.Event);
            return View(competitionEventDetails.ToList());
        }

        // GET: CompetitionEventDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionEventDetails competitionEventDetails = db.CompetitionEventDetails.Find(id);
            if (competitionEventDetails == null)
            {
                return HttpNotFound();
            }
            return View(competitionEventDetails);
        }

        // GET: CompetitionEventDetails/Create
        public ActionResult Create()
        {
            ViewBag.CompetitionID = new SelectList(db.Competitions, "ID", "Name");
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name");
            return View();
        }

        // POST: CompetitionEventDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CompetitionID,EventID,Round,CutOffDetails,TimeLimit,HowManyProced,StartTime,EndTime")] CompetitionEventDetails competitionEventDetails)
        {
            if (ModelState.IsValid)
            {
                db.CompetitionEventDetails.Add(competitionEventDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompetitionID = new SelectList(db.Competitions, "ID", "Name", competitionEventDetails.CompetitionID);
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name", competitionEventDetails.EventID);
            return View(competitionEventDetails);
        }

        // GET: CompetitionEventDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionEventDetails competitionEventDetails = db.CompetitionEventDetails.Find(id);
            if (competitionEventDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompetitionID = new SelectList(db.Competitions, "ID", "Name", competitionEventDetails.CompetitionID);
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name", competitionEventDetails.EventID);
            return View(competitionEventDetails);
        }

        // POST: CompetitionEventDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CompetitionID,EventID,Round,CutOffDetails,TimeLimit,HowManyProced,StartTime,EndTime")] CompetitionEventDetails competitionEventDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competitionEventDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompetitionID = new SelectList(db.Competitions, "ID", "Name", competitionEventDetails.CompetitionID);
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name", competitionEventDetails.EventID);
            return View(competitionEventDetails);
        }

        // GET: CompetitionEventDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionEventDetails competitionEventDetails = db.CompetitionEventDetails.Find(id);
            if (competitionEventDetails == null)
            {
                return HttpNotFound();
            }
            return View(competitionEventDetails);
        }

        // POST: CompetitionEventDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompetitionEventDetails competitionEventDetails = db.CompetitionEventDetails.Find(id);
            db.CompetitionEventDetails.Remove(competitionEventDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
