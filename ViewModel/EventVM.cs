using GlsLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.ViewModel
{
    public class EventVM
    {
        public Event Event { get; set; }
        public List<Event> EventsList { get; set; }

    }
}