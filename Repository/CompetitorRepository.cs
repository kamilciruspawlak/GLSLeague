﻿using GlsLeague.Models;
using GlsLeague.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.Repository
{
    public class CompetitorRepository : AbstractRepository<Competitor>, ICompetitorRepository
    {
    }
}