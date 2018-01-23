using GlsLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.ViewModel
{
    public class CompetitionsList
    {
        public List<Competition> AllCompetitionsList { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        public CompetitionsList()
        {
            AllCompetitionsList = db.Competitions.ToList();
        }

    }
}