using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlsLeague.Models;
using GlsLeague.Repository.Interfaces;
using GlsLeague.ViewModel;

namespace GlsLeague.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private IEventsRepository _eventsRepository;
        public EventsController(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        // GET: Events
        public ActionResult Index()
        {
            var eventVM = new EventVM();
            eventVM.EventsList = _eventsRepository.GetWhere(x => x.ID > 0);

            return View(eventVM);
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eventVM = new EventVM();
            eventVM.Event = _eventsRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();

            if (eventVM == null)
            {
                return HttpNotFound();
            }
            return View(eventVM);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventVM eventEntity)
        {
            if (ModelState.IsValid)
            {
                var eventVM = new EventVM();
                _eventsRepository.Create(eventEntity.Event);
                return RedirectToAction("Index");
            }

            return View(eventEntity);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eventVM = new EventVM();
            eventVM.Event = _eventsRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();
            if (eventVM == null)
            {
                return HttpNotFound();
            }
            return View(eventVM);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventVM eventEntity)
        {
            if (ModelState.IsValid)
            {
                _eventsRepository.Update(eventEntity.Event);
                return RedirectToAction("Index");
            }
            return View(eventEntity);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eventVM = new EventVM();
            eventVM.Event = _eventsRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();
            if (eventVM == null)
            {
                return HttpNotFound();
            }
            return View(eventVM);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event eventEntity = _eventsRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            _eventsRepository.Delete(eventEntity);
            return RedirectToAction("Index");
        }

   }
}
