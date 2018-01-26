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
using GlsLeague.Repository.Interfaces;

namespace GlsLeague.Controllers
{
    [Authorize]
    public class CompetitionsController : Controller
    {
        private ICompetitionsRepository _competitionsRepository;
        private IEventsRepository _eventsRepository;
        private ICompetitionEventsRepository _comeptitionEventsRepository;
        private ICompetitionEventDetailsRepository _competitionEventDetailsRepository;

        public CompetitionsController(ICompetitionsRepository competitionsRepository, IEventsRepository eventsRepository, ICompetitionEventsRepository comeptitionEventsRepository, ICompetitionEventDetailsRepository competitionEventDetailsRepository)
        {
            _competitionEventDetailsRepository = competitionEventDetailsRepository;
            _competitionsRepository = competitionsRepository;
            _eventsRepository = eventsRepository;
            _comeptitionEventsRepository = comeptitionEventsRepository;
        }
        // GET: Competitions
        public ActionResult Index()
        {
            var competitionVM = new CompetitionVM();
            competitionVM.CompetitionList = new List<Competition>();
            competitionVM.CompetitionList = _competitionsRepository.GetWhere(x => x.ID > 0);

            return View(competitionVM);
        }

        // GET: Competitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competitionVM = new CompetitionVM();
            competitionVM.Competition = _competitionsRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();

            if (competitionVM == null)
            {
                return HttpNotFound();
            }
            return View(competitionVM);
        }

        // GET: Competitions/Create
        public ActionResult Create()
        {

            var competitionVM = new CompetitionVM();
            competitionVM.EventsList = _eventsRepository.GetWhere(x => x.ID > 0);
            

            return View(competitionVM);
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetitionVM viewModel, int[] test )
        {
            if (ModelState.IsValid)
            {
                viewModel.Competition.IsCompetitionActvie = false;
                viewModel.Competition.IsRegistrationOpen = false;
                _competitionsRepository.Create(viewModel.Competition);
                
                viewModel.EventRoundNumberList.RemoveAll(x => string.IsNullOrEmpty(x));


                for (int i = 0; i < viewModel.SelectedEventsList.Count; i++)
               
                {
                    var competitionsEventsVM = new CompetitionEvents();
                    
                    competitionsEventsVM.CompetitionID = viewModel.Competition.ID;

                    competitionsEventsVM.EventID = int.Parse(viewModel.SelectedEventsList[i]);
                  
                    competitionsEventsVM.RoundNumber = int.Parse(viewModel.EventRoundNumberList[i]);

                    _comeptitionEventsRepository.Create(competitionsEventsVM);
                    
                }
                
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
            var competitionVM = new CompetitionVM();
            competitionVM.Competition = _competitionsRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();
            if (competitionVM == null)
            {
                return HttpNotFound();
            }
            return View(competitionVM);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetitionVM viewModel)
        {
            if (ModelState.IsValid)
            {
                _competitionsRepository.Update(viewModel.Competition);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Competitions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competitionVM = new CompetitionVM();
            competitionVM.Competition = _competitionsRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();
            if (competitionVM == null)
            {
                return HttpNotFound();
            }
            return View(competitionVM);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Competition competition = _competitionsRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            _competitionsRepository.Delete(competition);
            return RedirectToAction("Index");
        }



        public ActionResult Schedule(int? id)
        {
            var competitionEventDetailsVM = new CompetitionEventDetailsVM();
            competitionEventDetailsVM.CompetitionEventList = new List<CompetitionEvents>();
            competitionEventDetailsVM.CompetitionEventList = _comeptitionEventsRepository.GetWhere(x => x.CompetitionID == id.Value).ToList();

            competitionEventDetailsVM.Competition = new Competition { ID = (int)id };

            competitionEventDetailsVM.CompetitionEventDetailsList = _competitionEventDetailsRepository.GetWhere(x => x.CompetitionID == id.Value).ToList();

            competitionEventDetailsVM.EventsList = new List<Event>();
            foreach (var item in competitionEventDetailsVM.CompetitionEventList)
            {
                competitionEventDetailsVM.EventsList.Add(_eventsRepository.GetWhere(x=>x.ID == item.EventID).FirstOrDefault());
            }

           return View(competitionEventDetailsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Schedule(CompetitionEventDetailsVM viewModel)
        {

            var competitionEventDetails = new CompetitionEventDetails();
            viewModel.CompetitionEventDetails.CompetitionID = viewModel.Competition.ID;
            _competitionEventDetailsRepository.Create(viewModel.CompetitionEventDetails);

            return RedirectToAction("Schedule");
        }
        public ActionResult DeleteFromSchedule(int id)
        {
            CompetitionEventDetails CompetitionEventDetails = _competitionEventDetailsRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            _competitionEventDetailsRepository.Delete(CompetitionEventDetails);
            return RedirectToAction("Schedule", new { id = CompetitionEventDetails.CompetitionID });
        }
    }
}
