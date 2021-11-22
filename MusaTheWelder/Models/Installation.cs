using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusaTheWelder.Models
{
    public class Installation
    {
        [Key]
        public int InstallationId { get; set; }
        public int SaleQuoteId { get; set; }
        public string DateInstalled { get; set; }
        public bool isInstalled { get; set; }

        public virtual SaleQuote SaleQuote { get; set; }
    }
}