using GlsLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.ViewModel
{
    public class CompetitionEventDetailsVM
    {
        public CompetitionEventDetails CompetitionEventDetails { get; set; }
        public List<CompetitionEventDetails> CompetitionEventDetailsList { get; set; }

        public List<CompetitionEvents> CompetitionEventList { get; set; }

        public List<Event> EventsList { get; set; }
        public Event Event { get; set; }

        public Competition Competition { get; set; }
    }
}