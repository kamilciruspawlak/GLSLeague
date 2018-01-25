using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlsLeague.Models
{
    public class CompetitionEventDetails
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Competition")]
        public int CompetitionID { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Event")]
        public int EventID { get; set; }


        public int Round { get; set; }
        public string CutOffDetails { get; set; }
        public string TimeLimit { get; set; }
        public string HowManyProced { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}