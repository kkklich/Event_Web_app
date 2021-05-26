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
    public class Meet_participantController : Controller
    {
        private MeetingsEntities db = new MeetingsEntities();

        // GET: Meet_participant
        public ActionResult Index()
        {
            var meet_participant = db.Meet_participant.Include(m => m.Meet).Include(m => m.Participant);
            return View(meet_participant.ToList());
        }

        // GET: Meet_participant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meet_participant meet_participant = db.Meet_participant.Find(id);
            if (meet_participant == null)
            {
                return HttpNotFound();
            }
            return View(meet_participant);
        }

        // GET: Meet_participant/Create
        public ActionResult Create()
        {
            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Descript");
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName");
            return View();
        }

        // POST: Meet_participant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Meet_participant,Id_participant,Id_meet")] Meet_participant meet_participant)
        {
            if (ModelState.IsValid)
            {
                db.Meet_participant.Add(meet_participant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Place", meet_participant.Id_meet);
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName", meet_participant.Id_participant);
            return View(meet_participant);
        }

        // GET: Meet_participant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meet_participant meet_participant = db.Meet_participant.Find(id);
            if (meet_participant == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Place", meet_participant.Id_meet);
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName", meet_participant.Id_participant);
            return View(meet_participant);
        }

        // POST: Meet_participant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Meet_participant,Id_participant,Id_meet")] Meet_participant meet_participant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meet_participant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Place", meet_participant.Id_meet);
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName", meet_participant.Id_participant);
            return View(meet_participant);
        }

        // GET: Meet_participant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meet_participant meet_participant = db.Meet_participant.Find(id);
            if (meet_participant == null)
            {
                return HttpNotFound();
            }
            return View(meet_participant);
        }

        // POST: Meet_participant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meet_participant meet_participant = db.Meet_participant.Find(id);
            db.Meet_participant.Remove(meet_participant);
            db.SaveChanges();
            return RedirectToAction("Index");
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
