using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Ical.Net;
using Ical.Net.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.CalendarComponents;
using WebCruiter.Candidate.Web.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebCruiter.Candidate.Web.Services
{
    public class CalService:ICalService
    {


        /// <summary>
        ///Generates calendar object using CalendarInviteObject
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResponseMessageResult</returns>
        public IHttpActionResult CalendarBookingFileResult(CalendarInviteViewModel model)
        {
            var bytes = GetCalendarBookingBytes(model);
            return IcsFileContentResult(model, bytes);
        }

        private IHttpActionResult IcsFileContentResult(CalendarInviteViewModel entity, MemoryStream memory)
        {
            var filename = entity.FileName;


            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(memory.ToArray())
            };

            var encoder = Encoding.GetEncoding("us-ascii", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
            string asciiFileName = encoder.GetString(encoder.GetBytes(filename));

            // Set content headers
            message.Content.Headers.ContentLength = memory.Length;
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = asciiFileName
            };

            return new System.Web.Http.Results.ResponseMessageResult(message);
        }

        /// <summary>
        /// Creates calendar Objects and events
        /// </summary>
        /// <param name="model"></param>
        /// <returns>a MemoryStream</returns>
        private MemoryStream GetCalendarBookingBytes(CalendarInviteViewModel model)
        {
            
            var iCal = new Calendar();

            
            var evt = iCal.Create<CalendarEvent>();
            evt.Summary = model.Title;
            evt.Start = new CalDateTime(model.StartDate);
            evt.End = new CalDateTime(model.EndDate);
            evt.Description = model.Body;
            evt.Location = model.Address;

            if (model.StartDate.TimeOfDay.Hours == 0)
            {
                evt.IsAllDay = true;
            }
            
            evt.Uid = new Guid().ToString();
            //evt.Organizer = new Organizer(organizer);
            evt.Alarms.Add(new Alarm
            {                Duration = new TimeSpan(30, 0, 0),
                Trigger = new Trigger(new TimeSpan(30, 0, 0)),
                Action = AlarmAction.Display,
                Description = "Reminder"
            });
            SerializationContext ctx = new SerializationContext();
            ISerializerFactory factory = new SerializerFactory();
            var serializer = factory.Build(iCal.GetType(), ctx) as IStringSerializer;

            var output = serializer.SerializeToString(iCal);
            var bytes = Encoding.UTF8.GetBytes(output);

            MemoryStream ms = new MemoryStream(bytes);
            return ms;
        }



        //public MemoryStream GenerateIcsFile()
        //{

        //    var iCal = new Calendar();

        //    // Create the event, and add it to the iCalendar
        //    var evt = iCal.Create<CalendarEvent>();

        //    // Set information about the event
        //    evt.Start = CalDateTime.Today.AddHours(8);
        //    evt.End = evt.Start.AddHours(18); // This also sets the duration
        //    evt.Description = "The event description";
        //    evt.Location = "Event location";
        //    evt.Summary = "18 hour event summary";

        //    // Set information about the second event
        //    evt = iCal.Create<CalendarEvent>();
        //    evt.Start = CalDateTime.Today.AddDays(5);
        //    evt.End = evt.Start.AddDays(1);
        //    evt.IsAllDay = true;
        //    evt.Summary = "All-day event";


        //    // Create a serialization context and serializer factory.
        //    // These will be used to build the serializer for our object.
        //    SerializationContext ctx = new SerializationContext();
        //    ISerializerFactory factory = new SerializerFactory();
        //    // Get a serializer for our object
        //    var serializer = factory.Build(iCal.GetType(), ctx) as IStringSerializer;

        //    var output = serializer.SerializeToString(iCal);
        //    var bytes = Encoding.UTF8.GetBytes(output);


            
        //}




    }
}