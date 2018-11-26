using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebScrach2.Models;

namespace WebScrach2.Controllers
{
    public class MoviesController : Controller
    {
        private amugh786_aziz_dbEntities db = new amugh786_aziz_dbEntities();
         
        [AllowAnonymous]
        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }

            var moviedetail = new MovieDetail
            {
                movie = movy,
                movieID = movy.MovieID
            };

            return View(moviedetail);
        }

        [Authorize(Roles = "admin")]
        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]

        public ActionResult Create([Bind(Include = "MovieID,Title,Description,Rating,Price,Image")] Movy movy)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movy);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Comment( MovieDetail movieDetail)
        {
            var com = new Comment
            {
                comment1 = movieDetail.comment,
                MovieID = movieDetail.movieID,
                UserID = db.Users.Where(x=>x.UserNameOrEmail == User.Identity.Name).FirstOrDefault().UserID,
                AddDate = DateTime.Now
            };
            db.Comments.Add(com);
            db.SaveChanges();

            return RedirectToAction("Details", "Movies", new { id = movieDetail.movieID });

            //return View(movieDetail);
        }


        [Authorize(Roles = "admin")]
        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }
                [Authorize(Roles = "admin")]

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "MovieID,Title,Description,Rating,Price,Image")] Movy movy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movy);
        }

       [Authorize(Roles = "admin")]

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]

        public ActionResult DeleteConfirmed(int id)
        {
            Movy movy = db.Movies.Find(id);
            db.Movies.Remove(movy);
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
