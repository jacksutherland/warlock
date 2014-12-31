using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warlock.Models;

namespace Warlock.Controllers
{
    public class IssuesController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index(int id)
        {
            Series series = db.Series.Find(id);
            IEnumerable<Issue> issues = db.Issues.Where(i => i.SeriesId == id).OrderBy(i => i.Number);

            ViewBag.SeriesId = id;
            ViewBag.SeriesName = series.Name;
            ViewBag.SeriesYear = series.StartDate.Year;

            return View(issues);
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            return View(new Issue() { SeriesId = id, ImageUrl = "../../images/comics/no-image.jpg" });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = issue.SeriesId });
            }

            return View(issue);
        }

        public ActionResult Detail(int id)
        {
            return View(db.Issues.Find(id));
        }

        public ActionResult Recent(int page = 1, string search = "")
        {
            List<Issue> model = new List<Issue>();

            if (string.IsNullOrWhiteSpace(search))
            {
                page--;

                int noOfIssues = 20;
                model = new List<Issue>();
                DateTime date = DateTime.UtcNow.AddDays(-7).Date;
                int issuesCount = db.Issues.Where(i => i.SaleDate >= date).Count();

                if ((issuesCount / noOfIssues) >= (page - 1))
                {
                    model = db.Issues.Where(i => i.SaleDate >= date).OrderByDescending(i => i.SaleDate).Skip(page * noOfIssues).Take(noOfIssues).ToList();
                }

                ViewBag.Page = page;
                ViewBag.PageCount = issuesCount / noOfIssues;
            }
            else
            {
                IEnumerable<Series> series = db.Series.Where(s => s.Name.Contains(search.Trim()));
                foreach (Series s in series)
                {
                    Issue issue = db.Issues.Where(i => i.SeriesId == s.Id).OrderByDescending(i => i.SaleDate).FirstOrDefault();

                    if (issue != null)
                    {
                        model.Add(issue);
                    }
                }

                ViewBag.Search = search;
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ToggleIssueOwned(int id)
        {
            Issue issue = db.Issues.Find(id);
            issue.Owned = !issue.Owned;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = issue.SeriesId });
        }
    }
}
