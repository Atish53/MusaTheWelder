using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MusaTheWelder.Models
{
    public class Sale
    {
        [Key]

        public int SaleId { get; set; }

        public string SaleDate { get; set; }

        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }

        public double SaleTotal { get; set; }

        public List<SaleDetail> SaleDetails { get; set; }
        public List<SaleQuote> SaleQuotes { get; set; }
    }
}