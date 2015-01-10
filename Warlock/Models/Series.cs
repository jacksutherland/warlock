using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Warlock.Models
{
    public class Series
    {
        public int Id { get; set; }

        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public int? CollectionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Volume { get; set; }

        public bool Ongoing { get; set; }

        [NotMapped]
        public int NumberOfIssues { get; set; }

        [NotMapped]
        public bool UnOwnedIssues { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }
    }
}