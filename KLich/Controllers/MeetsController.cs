using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KLich;

namespace KLich.Controllers
{
    public class MeetsController : Controller
    {
        private MeetingsEntities db = new MeetingsEntities();

        // GET: Meets
        public ActionResult Index()
        {

            var meet_participant = db.Meet_participant.Include(m => m.Meet).Include(m => m.Participant);

            var linqNrPart = meet_participant.GroupBy(plac => plac.Meet.Descript)
                             .Select(plac => new
                             {
                                 Descript = plac.Key,
                                 NumberOfParticipant = plac.Count()
                             }).OrderBy(plac => plac.NumberOfParticipant);

            var meetsList = db.Meets;

            foreach(var item in meetsList)
            {
                
                var linqMeet_PArt = from x in meet_participant
                                    where x.Meet.Id_meet == item.Id_meet
                                    select x;

                int CountParticipant = linqMeet_PArt.Count();
                item.NumberOfParticipant = CountParticipant;

            }


            return View(meetsList.ToList());
        }

        // GET: Meets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var meet_participant = db.Meet_participant.Include(m => m.Meet).Include(m => m.Participant);
    

            var linqMeet_PArt = from x in meet_participant
                                where x.Meet.Id_meet == id
                                select x;

            var CountParticipant = linqMeet_PArt.Count();


            ViewBag.Count = CountParticipant;

            if (linqMeet_PArt == null)
            {
                return HttpNotFound();
            }
            Meet meetName = db.Meets.Find(id);
            ViewBag.EventName = meetName.Descript;

            return View(linqMeet_PArt);
        }

        // GET: Meets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Meets/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_meet,Place,Descript,DateEvent")] Meet meet)
        {
            if (ModelState.IsValid)
            {
                db.Meets.Add(meet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(meet);
        }

        // GET: Meets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meet meet = db.Meets.Find(id);
            if (meet == null)
            {
                return HttpNotFound();
            }
            return View(meet);
        }

        // POST: Meets/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_meet,Place,Descript,DateEvent")] Meet meet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(meet);
        }

        // GET: Meets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meet meet = db.Meets.Find(id);
            if (meet == null)
            {
                return HttpNotFound();
            }

       
            ViewBag.eventName = meet.Descript;
            return View(meet);
        }

        // POST: Meets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Meet meet = db.Meets.Find(id);
                db.Meets.Remove(meet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception f)
            {
                ViewBag.info = "BŁĄD nie można usunąć wydarzenia";

                
                Meet meet = db.Meets.Find(id);
                if (meet == null)
                {
                    return HttpNotFound();
                }
                return View(meet);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
