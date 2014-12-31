using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using Warlock.Models;

namespace Warlock.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private DataContext db = new DataContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(User.Identity.Name != "jacksutherl")
            {
                filterContext.Result = RedirectToAction("Index", "Home");                
            }

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Scrape(string url = "http://www.comicvine.com/new-comics/")
        {
            int newIssueCount = 0;
            IEnumerable<HtmlNode> issues = new List<HtmlNode>(); ;

            try
            {
                HtmlWeb html = new HtmlWeb();
                HtmlDocument doc = html.Load(url);
                HtmlNode list = doc.DocumentNode.Descendants("ul").Where(ul =>
                    ul.Attributes.Contains("class") && ul.Attributes["class"].Value.Contains("issue-grid")).FirstOrDefault();
                issues = list.ChildNodes.Where(i => i.Name == "li");

                foreach (HtmlNode li in issues)
                {
                    string date = li.Descendants("p").Where(p => p.Attributes.Contains("class") && p.Attributes["class"].Value.Contains("issue-date")).FirstOrDefault().InnerText;
                    string img = li.Descendants("img").FirstOrDefault().Attributes["src"].Value;
                    string name = li.Descendants("p").Where(p => p.Attributes.Contains("class") && p.Attributes["class"].Value.Contains("issue-number")).FirstOrDefault().InnerText;
                    string number = name.Substring(name.IndexOf('#') + 1);
                    if (number.IndexOf(' ') >= 0)
                    {
                        number = number.Substring(0, number.IndexOf(' '));
                    }
                    name = name.Substring(0, name.IndexOf("#") - 1);

                    double dNum;

                    if (double.TryParse(number, out dNum))
                    {
                        Series series = db.Series.Where(s => s.Name == name).FirstOrDefault();
                        if (series == null)
                        {
                            series = new Series()
                            {
                                PublisherId = 1,
                                StartDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value,
                                Name = name
                            };
                            db.Series.Add(series);
                            db.SaveChanges();
                        }

                        Issue issue = db.Issues.Where(i => i.SeriesId == series.Id && i.Number == dNum).FirstOrDefault();

                        if (issue == null)
                        {
                            db.Issues.Add(new Issue()
                            {
                                Number = dNum,
                                ImageUrl = img,
                                SaleDate = DateTime.Parse(date),
                                SeriesId = series.Id,
                                Price = 0
                            });
                            db.SaveChanges();
                            newIssueCount++;
                        }
                        else
                        {
                            issue.Number = double.Parse(number);
                            issue.ImageUrl = img;
                            db.SaveChanges();
                        }
                    }
                }

            }
            catch(Exception ex){}

            ViewBag.Url = url;
            ViewBag.IssueCount = issues.Count();
            ViewBag.NewIssueCount = newIssueCount;

            return View();
        }
    }
}
