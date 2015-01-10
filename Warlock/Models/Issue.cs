using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Warlock.Models
{
    public enum IssueDisplayType { All = 1, Owned = 2, Needed = 3 };

    public class Issue
    {
        public int Id { get; set; }

        [ForeignKey("Series")]
        public int SeriesId { get; set; }
        public virtual Series Series { get; set; }
        
        public double Number { get; set; }

        public string Description { get; set; }

        public DateTime? SaleDate { get; set; }

        public decimal? Price { get; set; }

        public string ImageUrl { get; set; }

        public string Writer { get; set; }

        public string Artist { get; set; }

        public string Colorist { get; set; }

        public string Letterist { get; set; }

        public bool Owned { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}