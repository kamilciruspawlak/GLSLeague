using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GlsLeague.Models
{
    public class Competition
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public bool IsCompetitionActvie { get; set; }
        public bool IsRegistrationOpen { get; set; }


        public virtual ICollection<CompetitionEvents> CompetitionEvents { get; set; }

    }
}