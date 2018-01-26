using GlsLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.ViewModel
{
    public class CompetitorVM
    {
        public Competitor Competitor { get; set; }
        public Competition Competition { get; set; }
        public int CompetitionID { get; set; }

        public List<CompetitionEvents> CompetitionEventsList { get; set; }

        public List<Event> EventsList { get; set; }
        public List<string> SelectedEventsList { get; set; }

    }
}