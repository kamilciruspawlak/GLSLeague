using GlsLeague.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.BusinessLogic.Interfaces
{
    public interface ICompetitionBusinessLogic
    {
        CompetitorVM GetAllInformationAboutCompetiion(int id);
    }
}