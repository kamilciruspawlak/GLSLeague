using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GlsLeague.Models
{
    public class Event
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        

        public virtual ICollection<CompetitionEvents> CompetitionEvents { get; set; }
        public virtual ICollection<CompetitorEvents> CompetitorEvents { get; set; }
    }
}