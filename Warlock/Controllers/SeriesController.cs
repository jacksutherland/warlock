using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warlock.Models;

namespace Warlock.Controllers
{
    public class SeriesController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Series/

        public ActionResult Index(string search = "")
        {
            IEnumerable<Series> series = new List<Series>();

            series = string.IsNullOrWhiteSpace(search) ?
                db.Series.OrderBy(s => s.StartDate):
                db.Series.Where(s => s.Name.Contains(search.Trim()));

            foreach (Series s in series)
            {
                s.UnOwnedIssues = db.Issues.Any(i => i.SeriesId == s.Id && !i.Owned);
                if (db.Issues.Any(i => i.SeriesId == s.Id))
                {
                    Issue issue = db.Issues.Where(i => i.SeriesId == s.Id).OrderBy(i => i.Number).First();
                    if (issue != null)
                    {
                        s.ImageUrl = issue.ImageUrl;
                    }
                }
            }

            return View(series);
        }

        //
        // GET: /Series/Details/5

        public ActionResult Details(int id = 0)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Series/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Series series)
        {
            if (ModelState.IsValid)
            {
                series.PublisherId = 1;
                series.CollectionId = 1;
                db.Series.Add(series);
                db.SaveChanges();

                if (series.NumberOfIssues != null && series.NumberOfIssues > 0)
                {
                    for(int i = 1; i <= series.NumberOfIssues; i++ )
                    {
                        db.Issues.Add(new Issue()
                        {
                            SeriesId = series.Id,
                            Number = i
                        });
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(series);
        }

        //
        // GET: /Series/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        //
        // POST: /Series/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Series series)
        {
            if (ModelState.IsValid)
            {
                db.Entry(series).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(series);
        }

        //
        // GET: /Series/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Series series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        //
        // POST: /Series/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Series series = db.Series.Find(id);
            db.Series.Remove(series);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}