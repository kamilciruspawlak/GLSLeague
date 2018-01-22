using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlsLeague.Models
{
    public class CompetitionEvents
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Competition")]
        public int CompetitionID { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Event")]
        public int EventID { get; set; }

        public int RoundNumber { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Event Event { get; set; }

    }
}