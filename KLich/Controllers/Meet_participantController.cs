using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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




        public ActionResult Add_Participant_Meet(int id)
        {
            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Descript");
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName");

            var eventName = db.Meets.Find(id);
            ViewBag.EventName = eventName.Descript;
            ViewBag.EventID = eventName.Id_meet;
            return View();
        }


        [HttpPost]
        public ActionResult Add_Participant_Meet([Bind(Include = "Id_participant,FirstName,SureName,Email")] Participant participant, string id_meet)
        {
            int int_id_meet = int.Parse(id_meet);
            var eventName = db.Meets.Find(int_id_meet);
            ViewBag.EventName = eventName.Descript;

            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Descript");
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName");

            
            //Oblicanie ile jest 
            var linqCount = from x in db.Meet_participant
                            where x.Id_meet == int_id_meet
                            select x;

            int meetCount = linqCount.Count();

            if (meetCount < 20)
            {
                try
                {
                    //Dodawanie użytkownika do bazy danych                      
                    db.Participants.Add(participant);
                    //db.SaveChanges();


                    //Dodawanie stworzonego przed chwilą użytkownika do istniejącego już wcześniej wydarzenia
                    Meet_participant meet_Participant1 = new Meet_participant();
                    meet_Participant1.Id_participant = participant.Id_participant;
                    meet_Participant1.Id_meet = int_id_meet;

                    db.Meet_participant.Add(meet_Participant1);
                    db.SaveChanges();

                    ViewBag.info = "Zapisano uczestnika " + participant.FirstName;
                }
                catch (Exception)
                {
                    ViewBag.info = "Błąd";
                    return View();
                }

            }
            else
            {
                ViewBag.info = "Zbyt dużo uczestników w wydarzeniu: "+meetCount.ToString();
                return View(); 
            }
            return RedirectToAction("Details", new RouteValueDictionary(
                      new { controller = "Meets", action = "Main", id = int_id_meet }));           
        }


        //Dodawanie uczestnika i wybieranie z listy wydarzenia
        public ActionResult ViewAdd()
        {
            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Descript");

            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName");
            return View();
        }

        [HttpPost]
        public ActionResult ViewAdd(string firstName, string sureName, string email, [Bind(Include = "id_Meet_participant,Id_meet")] Meet_participant meet_participant)
        {

            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Descript");
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName");
         
            //Obliczanie ile jest 
            var linqCount = from x in db.Meet_participant
                            where x.Id_meet == meet_participant.Id_meet
                            select x;

            int meetCount = linqCount.Count();

            if (meetCount < 20 & firstName!=null & email!=null & meet_participant.Id_meet.ToString()!=null)
            {
                try
                {
                    //Dodawanie użytkownika do bazy danych
                    Participant participant1 = new Participant();
                    participant1.FirstName = firstName;
                    participant1.SureName = sureName;
                    participant1.Email = email;

                    db.Participants.Add(participant1);
                    
                    ViewBag.Id_participant = participant1.Id_participant.ToString();


                    //Dodawanie stworzonego przed chwilą użytkownika do istniejącego już wcześniej wydarzenia
                    Meet_participant meet_Participant1 = new Meet_participant();
                    meet_Participant1.Id_participant = participant1.Id_participant;
                    meet_Participant1.Id_meet = meet_participant.Id_meet;

                    db.Meet_participant.Add(meet_Participant1);
                    db.SaveChanges();

                    ViewBag.info = "Zapisano uczestnika " + participant1.FirstName;

                }
                catch(Exception )
                {
                    ViewBag.info = "Błąd";
                    return View();
                }

            }else
            {
                ViewBag.info = "Zbyt dużo uczestników w wydarzeniu";
            }

            return View();
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


            ViewBag.Id_meet = new SelectList(db.Meets, "Id_meet", "Descript", meet_participant.Id_meet);
            ViewBag.Id_participant = new SelectList(db.Participants, "Id_participant", "FirstName", meet_participant.Id_participant);

            if (ModelState.IsValid)
            {              

                var linqMeet_Count = from x in db.Meet_participant
                                     where x.Meet.Id_meet == meet_participant.Id_meet
                                     select x;

                int countMeet = linqMeet_Count.Count();

                if (countMeet < 20)
                {
                    db.Meet_participant.Add(meet_participant);
                    db.SaveChanges();
                    ViewBag.info = "Zapisano do bazy ";

                }else
                {
                    ViewBag.info = "Za dużo uczestników ";
                }

                ViewBag.count =" liczba uczestników: "+countMeet.ToString();



                return View();
                //return RedirectToAction("Index");
            }

            
            return View();
            //return View(meet_participant);
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
           // return RedirectToAction("Details","Meets", new { id = 5 });

            return RedirectToAction("Details", new RouteValueDictionary(
                      new { controller = "Meets", action = "Main", id = meet_participant.Id_meet }));

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
