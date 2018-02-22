using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using WebCruiter.Candidate.Web.Models;

namespace WebCruiter.Candidate.Web.Services
{
    public interface ICalService
    {
        IHttpActionResult CalendarBookingFileResult(CalendarInviteViewModel model);
        //MemoryStream GenerateIcsFile();
    }
}
