using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusaTheWelder.Models
{
    public class SaleQuote
    {
        //
        public int SaleQuoteId { get; set; }
        public int SaleId { get; set; }       
        public virtual Sale Sale { get; set; }

        //
        [DataType(DataType.MultilineText)]
        public string QuoteInstructions { get; set; }
        public string Status { get; set; }
        public double QuotePrice { get; set; }

        //Quote Status
        public bool isAccepted { get; set; }
        public bool isDeclined { get; set; }
        public bool isPaid { get; set; }        
    }
}