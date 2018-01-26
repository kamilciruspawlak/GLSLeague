using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GlsLeague.Models
{
    public class Competitor
    {
        public int ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WCAIdetificator { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }

        public ICollection<CompetitorEvents> CompetitorEvents { get; set; }
    }
}