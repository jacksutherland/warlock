using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warlock.Models;

namespace Warlock.Controllers
{
    public class CollectionController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            return View(db.Collections.ToList());
        }

        public ActionResult Series(int id)
        {
            Collection collection = db.Collections.Find(id);
            IEnumerable<Series> series = db.Series.Where(s => s.CollectionId == id);

            ViewBag.CollectionName = collection.Name;

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

        public ActionResult Issues(int id, IssueDisplayType show = IssueDisplayType.All)
        {
            Collection collection = db.Collections.Find(id);

            ViewBag.Collection = collection;
            ViewBag.DisplayType = show;

            IEnumerable<Issue> issues;

            switch (show)
            {
                case IssueDisplayType.Owned:
                    issues = db.Issues.Where(i => i.Series.CollectionId == id && i.Owned).OrderBy(i => i.SaleDate);
                    break;
                case IssueDisplayType.Needed:
                    issues = db.Issues.Where(i => i.Series.CollectionId == id && !i.Owned).OrderBy(i => i.SaleDate);
                    break;
                default:
                    issues = db.Issues.Where(i => i.Series.CollectionId == id).OrderBy(i => i.SaleDate);
                    break;
            }

            return View(issues);
        }

    }
}
