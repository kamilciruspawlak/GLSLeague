using GlsLeague.Models;
using GlsLeague.Repository.Interfaces;
using GlsLeague.BusinessLogic.Interfaces;
using GlsLeague.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.BusinessLogic
{
    public class CompetitionBusinessLogic : ICompetitionBusinessLogic
    {
        private ICompetitionsRepository _competitionRepository;
        private ICompetitionEventsRepository _competitionEventsRepository;
        private IEventsRepository _eventsRepository;
        private ICompetitorRepository _competitorRepository;
        private ICompetitorEventsRepository _competitorEventsRepository;

        public CompetitionBusinessLogic(ICompetitionsRepository competitionsRepository, ICompetitionEventsRepository competitionEventsRepository, IEventsRepository eventsRepository, ICompetitorRepository competitorRepository, ICompetitorEventsRepository competitorEventsRepository)
        {
            _competitionRepository = competitionsRepository;
            _competitionEventsRepository = competitionEventsRepository;
            _eventsRepository = eventsRepository;
            _competitorRepository = competitorRepository;
            _competitorEventsRepository = competitorEventsRepository;
        }
       public CompetitorVM GetAllInformationAboutCompetiion(int id)
        {
            var competitorVM = new CompetitorVM();
            competitorVM.Competition = new Competition();
            competitorVM.CompetitorsList = new List<Competitor>();
            competitorVM.CompetitorEventsList = new List<CompetitorEvents>();
            competitorVM.CompetitionEventsList = new List<CompetitionEvents>();
            competitorVM.Competitor = new Competitor();
            competitorVM.Competitor.CompetitorEvents = new List<CompetitorEvents>();
            competitorVM.EventsList = new List<Event>();

            competitorVM.Competition = _competitionRepository.GetWhere(x => x.ID == id).FirstOrDefault();
            competitorVM.CompetitionEventsList = _competitionEventsRepository.GetWhere(x => x.CompetitionID == id).ToList();
            foreach (var item in competitorVM.CompetitionEventsList)
            {
                competitorVM.EventsList.Add(_eventsRepository.GetWhere(x => x.ID == item.EventID).FirstOrDefault());
            }

            competitorVM.CompetitorEventsList = _competitorEventsRepository.GetWhere(x => x.CompetitionID == id).ToList();


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

            int numberOfEventsOnCompetition = competitorVM.CompetitionEventsList.Count;
            foreach (var item in competitorVM.CompetitorsList)
            {
                item.CompetitorEvents = item.CompetitorEvents.ToList();
                var tempCompetitorEventsList = new List<CompetitorEvents>();
                var competitorlist = new List<CompetitorEvents>();
                int j = 0;
                for (int i = 0; i < numberOfEventsOnCompetition; i++)
                {
                    competitorlist = item.CompetitorEvents.ToList();
                    for (int y = 0; y < numberOfEventsOnCompetition - competitorlist.Count; y++)
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
            return competitorVM;
        }

        
    }
}
