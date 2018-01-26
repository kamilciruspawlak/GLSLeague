using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlsLeague.Models
{
    public class CompetitorEvents
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Competition")]
        public int CompetitionID { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Event")]
        public int EventID { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("Competitor")]
        public int CompetitiorID { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Event Event { get; set; }
        public virtual Competitor Competitor { get; set; }
    }
}