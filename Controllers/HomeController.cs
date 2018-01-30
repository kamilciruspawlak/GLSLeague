using GlsLeague.BusinessLogic;
using GlsLeague.BusinessLogic.Interfaces;
using GlsLeague.Models;
using GlsLeague.Repository.Interfaces;
using GlsLeague.ViewModel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlsLeague.Controllers
{
    public class HomeController : Controller
    {
        private ICompetitionsRepository _competitionRepository;
        private ICompetitionEventsRepository _competitionEventsRepository;
        private IEventsRepository _eventsRepository;
        private ICompetitorRepository _competitorRepository;
        private ICompetitorEventsRepository _competitorEventsRepository;
        private ICompetitionBusinessLogic _competitionBusinessLogic;
        private ICompetitionEventDetailsRepository _competitionEventDetailsRepository;

        public HomeController(ICompetitionsRepository competitionsRepository, ICompetitionEventsRepository competitionEventsRepository, IEventsRepository eventsRepository, ICompetitorRepository competitorRepository, ICompetitorEventsRepository competitorEventsRepository, ICompetitionBusinessLogic competitionBusinessLogic,ICompetitionEventDetailsRepository competitionEventDetailsRepository)
        {
            _competitionRepository = competitionsRepository;
            _competitionEventsRepository = competitionEventsRepository;
            _eventsRepository = eventsRepository;
            _competitorRepository = competitorRepository;
            _competitorEventsRepository = competitorEventsRepository;
            _competitionBusinessLogic = competitionBusinessLogic;
            _competitionEventDetailsRepository = competitionEventDetailsRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ActiveCompetitions(CompetitionVM viewModel)
        {
            viewModel.CompetitionList = _competitionRepository.GetWhere(x => x.IsCompetitionActvie == true).ToList();

            ViewBag.Message = "Your contact page.";

            return View(viewModel);
        }

        public ActionResult CompetitionDetails(int? id)
        {
            var competitionVM = new CompetitionVM();
            competitionVM.Competition = _competitionRepository.GetWhere(x => x.ID == id.Value).FirstOrDefault();
            return View(competitionVM);
        }
        public ActionResult Register(int? id)
        {
            var competitorVM = new CompetitorVM();
            competitorVM.CompetitionID = id.Value;
            competitorVM.CompetitionEventsList = _competitionEventsRepository.GetWhere(x => x.CompetitionID == id.Value).ToList();

            competitorVM.EventsList = new List<Event>();
            foreach (var item in competitorVM.CompetitionEventsList)
            {
                competitorVM.EventsList.Add(_eventsRepository.GetWhere(x => x.ID == item.EventID).FirstOrDefault());
            }
            return View(competitorVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CompetitorVM viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Competitor.IsConfirmed = false;
                _competitorRepository.Create(viewModel.Competitor);


                foreach (var item in viewModel.SelectedEventsList)
                {
                    var competitorEvents = new CompetitorEvents();
                    competitorEvents.CompetitiorID = viewModel.Competitor.ID;
                    competitorEvents.EventID = int.Parse(item);
                    competitorEvents.CompetitionID = viewModel.CompetitionID;
                    _competitorEventsRepository.Create(competitorEvents);
                }

            }
            return RedirectToAction("RegisterConfirm");
        }
        public ActionResult RegisterConfirm()
        {
            return View();
        }
        public ActionResult Registered(int? id)
        {
            var competitionVM = new CompetitorVM();
            competitionVM = _competitionBusinessLogic.GetAllInformationAboutCompetiion(id.Value);

            return View(competitionVM);
        }
        public ActionResult Schedule(int id)
        {
            var viewModel = new CompetitionEventDetailsVM();
            viewModel.CompetitionEventDetailsList = _competitionEventDetailsRepository.GetWhere(x => x.CompetitionID == id).ToList();
            
            viewModel.CompetitionEventDetailsList.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

            return View(viewModel);
        }
    }
}