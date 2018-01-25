using GlsLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlsLeague.ViewModel
{
    public class CompetitionVM
    {
        public Competition Competition { get; set; }
        public List<Competition> CompetitionList { get; set; }

        public CompetitionEvents CompetitionEvents { get; set; }

        public List<Event> EventsList { get; set; }
        public List<string> SelectedEventsList { get; set; }

        public List<string> EventRoundNumberList { get; set; }

        
        
       
    }
}