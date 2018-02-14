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
using GlsLeague.BusinessLogic.Interfaces;
using System.IO;
using GlsLeague.Validation;

namespace GlsLeague.Controllers
{
    [Authorize]
    public class CompetitionsController : Controller
    {
        private ICompetitionsRepository _competitionsRepository;
        private IEventsRepository _eventsRepository;
        private ICompetitionEventsRepository _comeptitionEventsRepository;
        private ICompetitionEventDetailsRepository _competitionEventDetailsRepository;
        private ICompetitionBusinessLogic _competitionBusinessLogic;
        private ICompetitorRepository _competitorRepository;
        private ICompetitorEventsRepository _competitorEventsRepository;

        public CompetitionsController(ICompetitionsRepository competitionsRepository, IEventsRepository eventsRepository, ICompetitionEventsRepository comeptitionEventsRepository, ICompetitionEventDetailsRepository competitionEventDetailsRepository, ICompetitionBusinessLogic competitionBusinessLogic, ICompetitorRepository competitorRepository, ICompetitorEventsRepository competitorEventsRepository)
        {
            _competitionEventDetailsRepository = competitionEventDetailsRepository;
            _competitionsRepository = competitionsRepository;
            _eventsRepository = eventsRepository;
            _comeptitionEventsRepository = comeptitionEventsRepository;
            _competitionBusinessLogic = competitionBusinessLogic;
            _competitorRepository = competitorRepository;
            _competitorEventsRepository = competitorEventsRepository;
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
        public ActionResult Create(CompetitionVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var validator = new CompetitionValidator();
                var result = validator.Validate(viewModel.Competition);

                if (result.Errors.Any())
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.ErrorMessage);
                    }
                }
                else
                {
                    if (viewModel.ImageFile != null)
                    {
                        string extension = Path.GetExtension(viewModel.ImageFile.FileName);
                        string fileName = viewModel.Competition.Name.Replace(" ", string.Empty) + DateTime.Now.ToString("yymmdd") + extension;
                        viewModel.Competition.PathImage = "~/Image/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                        viewModel.ImageFile.SaveAs(fileName);
                    }

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
            }
            viewModel.EventsList = _eventsRepository.GetWhere(x => x.ID > 0);
            return View(viewModel);
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
            competitionEventDetailsVM.CompetitionEventDetailsList.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

            competitionEventDetailsVM.EventsList = new List<Event>();
            foreach (var item in competitionEventDetailsVM.CompetitionEventList)
            {
                competitionEventDetailsVM.EventsList.Add(_eventsRepository.GetWhere(x => x.ID == item.EventID).FirstOrDefault());
            }

            return View(competitionEventDetailsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Schedule(CompetitionEventDetailsVM viewModel)
        {

            var competitionEventDetails = new CompetitionEventDetails();
            viewModel.CompetitionEventDetails.CompetitionID = viewModel.Competition.ID;
            viewModel.CompetitionEventDetails.Round = int.Parse(viewModel.Round);
            _competitionEventDetailsRepository.Create(viewModel.CompetitionEventDetails);

            return RedirectToAction("Schedule");
        }
        public ActionResult DeleteFromSchedule(int id)
        {
            CompetitionEventDetails CompetitionEventDetails = _competitionEventDetailsRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            _competitionEventDetailsRepository.Delete(CompetitionEventDetails);

            return RedirectToAction("Schedule", new { id = CompetitionEventDetails.CompetitionID });
        }

        public ActionResult Registered(int id)
        {
            var competitionVM = new CompetitorVM();
            competitionVM = _competitionBusinessLogic.GetAllInformationAboutCompetiion(id);

            return View(competitionVM);
        }
        public ActionResult ConfirmCompetitor(int id, int competitionId)
        {
            Competitor competitor = _competitorRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            if (competitor.IsConfirmed != true)
            {
                competitor.IsConfirmed = true;
            }
            else
            {
                competitor.IsConfirmed = false;
            }
            _competitorRepository.Update(competitor);

            return RedirectToAction("Registered", new { id = competitionId });
        }
        public ActionResult DeleteCompetitor(int id, int competitionId)
        {
            Competitor competitor = _competitorRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            _competitorRepository.Delete(competitor);

            return RedirectToAction("Registered", new { id = competitionId });
        }
        public ActionResult EditCompetitor(int id, int competitionId)
        {
            var competitorVM = new CompetitorVM();
            competitorVM.CompetitionEventsList = new List<CompetitionEvents>();



            competitorVM.Competitor = _competitorRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            competitorVM.CompetitionID = competitionId;

            competitorVM.CompetitionEventsList = _comeptitionEventsRepository.GetWhere(x => x.CompetitionID == competitionId).ToList();


            competitorVM.EventsList = new List<Event>();
            foreach (var item in competitorVM.CompetitionEventsList)
            {
                competitorVM.EventsList.Add(_eventsRepository.GetWhere(x => x.ID == item.EventID).FirstOrDefault());
            }

            if (competitorVM.Competitor == null)
            {
                return HttpNotFound();
            }
            return View(competitorVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompetitor(CompetitorVM competitorVM)
        {
            if (ModelState.IsValid)
            {
                var temporaryCompetitionEventsList = new List<CompetitorEvents>();
                var temporaryCompetitorEventsList = new List<CompetitorEvents>();
                temporaryCompetitionEventsList = _competitorEventsRepository.GetWhere(x => x.CompetitionID == competitorVM.CompetitionID).ToList();
                temporaryCompetitorEventsList = temporaryCompetitionEventsList.Where(x => x.CompetitiorID == competitorVM.Competitor.ID).ToList();

                foreach (var item in competitorVM.SelectedEventsList)
                {
                    if (temporaryCompetitorEventsList.Find(x => x.EventID == int.Parse(item)) == null)
                    {
                        var competitorEvents = new CompetitorEvents();
                        competitorEvents.CompetitiorID = competitorVM.Competitor.ID;
                        competitorEvents.EventID = int.Parse(item);
                        competitorEvents.CompetitionID = competitorVM.CompetitionID;
                        _competitorEventsRepository.Create(competitorEvents);
                    }
                    else
                    {
                        var competitorEvents = temporaryCompetitorEventsList.Find(x => x.EventID == int.Parse(item));
                        _competitorEventsRepository.Delete(competitorEvents);
                    }

                }
                _competitorRepository.Update(competitorVM.Competitor);
                return RedirectToAction("Registered", new { id = competitorVM.CompetitionID });
            }
            return View(competitorVM);
        }
    }
}
