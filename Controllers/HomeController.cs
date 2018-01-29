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

        public HomeController(ICompetitionsRepository competitionsRepository, ICompetitionEventsRepository competitionEventsRepository, IEventsRepository eventsRepository, ICompetitorRepository competitorRepository, ICompetitorEventsRepository competitorEventsRepository)
        {
            _competitionRepository = competitionsRepository;
            _competitionEventsRepository = competitionEventsRepository;
            _eventsRepository = eventsRepository;
            _competitorRepository = competitorRepository;
            _competitorEventsRepository = competitorEventsRepository;
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
            var competitiorVM = new CompetitorVM();
            competitiorVM.CompetitionID = id.Value;
            competitiorVM.CompetitionEventsList = _competitionEventsRepository.GetWhere(x => x.CompetitionID == id.Value).ToList();

            competitiorVM.EventsList = new List<Event>();
            foreach (var item in competitiorVM.CompetitionEventsList)
            {
                competitiorVM.EventsList.Add(_eventsRepository.GetWhere(x => x.ID == item.EventID).FirstOrDefault());
            }
            return View(competitiorVM);
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
            var competitorVM = new CompetitorVM();
            competitorVM.CompetitorsList = new List<Competitor>();
            competitorVM.CompetitorEventsList = new List<CompetitorEvents>();
            competitorVM.CompetitionEventsList = new List<CompetitionEvents>();
            competitorVM.Competitor = new Competitor();
            competitorVM.Competitor.CompetitorEvents = new List<CompetitorEvents>();
            competitorVM.EventsList = new List<Event>();
            

            competitorVM.CompetitionEventsList = _competitionEventsRepository.GetWhere(x => x.CompetitionID == id.Value).ToList();
            foreach (var item in competitorVM.CompetitionEventsList)
            {
                competitorVM.EventsList.Add(_eventsRepository.GetWhere(x => x.ID == item.EventID).FirstOrDefault());
            }

            competitorVM.CompetitorEventsList = _competitorEventsRepository.GetWhere(x => x.CompetitionID == id.Value).ToList();
           

            var newCompetitorList = competitorVM.CompetitorEventsList.GroupBy(cel => cel.CompetitiorID).ToList();

            foreach (var item in newCompetitorList)
            {
                competitorVM.CompetitorsList.Add(_competitorRepository.GetWhere(x => x.ID == item.Key).FirstOrDefault());
            }




            foreach (var item in competitorVM.CompetitorsList)
            {
                item.CompetitorEvents = new List<CompetitorEvents>();
                foreach (var cos in competitorVM.CompetitorEventsList)
                {
                    if (item.ID == cos.CompetitiorID)
                    {
                        item.CompetitorEvents.Add(cos);
                    }

                }
            }

                 int liczba = competitorVM.CompetitionEventsList.Count;
                foreach (var item in competitorVM.CompetitorsList)
                {
                    item.CompetitorEvents = item.CompetitorEvents.ToList();
                    var tempCompetitorEventsList = new List<CompetitorEvents>();
                    var competitorlist = new List<CompetitorEvents>();
                    int j = 0;
                    for (int i = 0; i < liczba; i++)    
                    {
                        competitorlist = item.CompetitorEvents.ToList();
                        for (int y = 0; y < liczba - competitorlist.Count; y++)
                        {
                            competitorlist.Add(new CompetitorEvents());
                        }
                        if (competitorlist[j].EventID == competitorVM.CompetitionEventsList[i].EventID)
                        {
                            tempCompetitorEventsList.Add(new CompetitorEvents() { EventID = competitorVM.CompetitionEventsList[i].EventID });
                            j++;
                            }
                        else
                        {
                            tempCompetitorEventsList.Add(new CompetitorEvents() { EventID = 999 });
                        }
                    }
                            item.CompetitorEvents = tempCompetitorEventsList;
                }


            //var tryNewViewModel = competitorVM.CompetitorEventsList.GroupBy(cel => cel.CompetitiorID).Select(g => new { Competitor = g.Key, ListOfTakenCompetitions = g }).ToList();

            //var newModelList = new List<MyNewViewModel>();
            //foreach (var item in tryNewViewModel)
            //{
            //    var vm = new MyNewViewModel()
            //    {
            //        FirstName = "asd",
            //        LastName = "asd",

            //        CompetitionTaken = new List<CompetitonTaken>()


            //    };

            //    vm.CompetitionTaken.Add(new CompetitonTaken()
            //    {
            //        EventId = 5
            //    });

            //    newModelList.Add(vm);




            //}

           var stolec = competitorVM.CompetitorsList.OrderByDescending(x => x.ID);
            competitorVM.CompetitorsList = stolec;
            return View(competitorVM);
        }
    }
}