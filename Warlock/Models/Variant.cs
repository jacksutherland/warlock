using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Warlock.Models
{
    public class Variant
    {
        public int Id { get; set; }

        [ForeignKey("Issue")]
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverArtist { get; set; }
    }
}