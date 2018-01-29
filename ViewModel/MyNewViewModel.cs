using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.ViewModel
{
    public class MyNewViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<CompetitonTaken> CompetitionTaken { get; set; }

    }
}