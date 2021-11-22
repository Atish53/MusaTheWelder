using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusaTheWelder.Models;
using System.Net.Mail;
using System.Text;

namespace MusaTheWelder.Controllers
{
    public class SaleQuotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SaleQuotes
        public async Task<ActionResult> Index()
        {
            var saleQuotes = db.SaleQuotes.Include(s => s.Sale);
            return View(await saleQuotes.ToListAsync());
        }

        // GET: SaleQuotes/Details/5
        public async Task<ActionResult> QuoteDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            if (saleQuote == null)
            {
                return HttpNotFound();
            }

            int ids = saleQuote.SaleId;

            var productList = from db in db.SaleDetails
                              where db.SaleId == ids
                              select db.Product;

            ViewBag.SaleDetails = productList;

            return View(saleQuote);
        }

        
        public async Task<ActionResult> AcceptQuote(int? id)
        {
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            saleQuote.Status = "Accepted";
            saleQuote.isAccepted = true;
            saleQuote.isPaid = true;
            saleQuote.isDeclined = false;

            Installation installation = new Installation();
            installation.SaleQuoteId = saleQuote.SaleQuoteId;
            installation.isInstalled = false;            

            int saleids = saleQuote.SaleId;

            Sale sale = await db.Sales.FindAsync(saleids);

            db.SaveChanges();

            //Payment...
            try
            {
                // Retrieve required values for the PayFast Merchant
                string name = "Musa's Welding Quote/Installation Number: #" + saleQuote.SaleQuoteId;
                string description = "This is a once-off and non-refundable payment. ";

                string site = "https://sandbox.payfast.co.za/eng/process";
                string merchant_id = "";
                string merchant_key = "";

                string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

                if (paymentMode == "test")
                {
                    site = "https://sandbox.payfast.co.za/eng/process?";
                    merchant_id = "10000100";
                    merchant_key = "46f0cd694581a";
                }

                // Build the query string for payment site

                StringBuilder str = new StringBuilder();
                str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
                str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
                str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
                str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
                str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

                str.Append("&m_payment_id=" + HttpUtility.UrlEncode(sale.SaleId.ToString()));
                str.Append("&amount=" + HttpUtility.UrlEncode(saleQuote.QuotePrice.ToString()));
                str.Append("&item_name=" + HttpUtility.UrlEncode(name));
                str.Append("&item_description=" + HttpUtility.UrlEncode(description));

                // Redirect to PayFast
                return Redirect(site + str.ToString());
            }
            catch (Exception)
            {
                throw;
            }            
        }

        
        public async Task<ActionResult> RejectQuote(int? id)
        {

            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);                       
            saleQuote.Status = "Rejected";
            saleQuote.isAccepted = false;
            saleQuote.isPaid = false;
            saleQuote.isDeclined = true;

            db.SaveChanges();
            
            return RedirectToAction("RejectedQuote");
        }


        // GET: SaleQuotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            if (saleQuote == null)
            {
                return HttpNotFound();
            }

            int ids = saleQuote.SaleId;

            var productList = from db in db.SaleDetails
                              where db.SaleId == ids
                              select db.Product;

            ViewBag.SaleDetails = productList;

            return View(saleQuote);
        }

        [HttpPost]
        public async Task<ActionResult> Details(string Install, int id)
        {
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            ViewData["Install"] = Install;

            string InstallData = Install;

            saleQuote.QuotePrice = Convert.ToDouble(InstallData);
            saleQuote.Status = "Responded";

            db.SaveChanges();

            MailMessage mail = new MailMessage();
            string emailTo = User.Identity.Name;
            MailAddress from = new MailAddress("21827882@dut4life.ac.za");
            mail.From = from;
            mail.Subject = "Your quote price for Quote ID #" + saleQuote.SaleQuoteId;
            mail.Body = "Dear " + saleQuote.Sale.CustomerName + ", your quote price is R" + saleQuote.QuotePrice + ". Please go to your purchase history to either accept or decline this offer.";
            mail.To.Add(emailTo);
            
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("21827882@dut4life.ac.za", "Dut991110");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);
            mail.Dispose();


            return RedirectToAction("Index");
        }


        // GET: SaleQuotes/Create
        public ActionResult Create()
        {
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleDate");
            return View();
        }

        // POST: SaleQuotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaleQuoteId,SaleId,QuoteInstructions,Status,QuotePrice,isAccepted,isDeclined,isPaid")] SaleQuote saleQuote)
        {
            if (ModelState.IsValid)
            {
                db.SaleQuotes.Add(saleQuote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleDate", saleQuote.SaleId);
            return View(saleQuote);
        }

        // GET: SaleQuotes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            if (saleQuote == null)
            {
                return HttpNotFound();
            }
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleDate", saleQuote.SaleId);
            return View(saleQuote);
        }

        // POST: SaleQuotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaleQuoteId,SaleId,QuoteInstructions,Status,QuotePrice,isAccepted,isDeclined,isPaid")] SaleQuote saleQuote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleQuote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleDate", saleQuote.SaleId);
            return View(saleQuote);
        }

        // GET: SaleQuotes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            if (saleQuote == null)
            {
                return HttpNotFound();
            }
            return View(saleQuote);
        }

        // POST: SaleQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SaleQuote saleQuote = await db.SaleQuotes.FindAsync(id);
            db.SaleQuotes.Remove(saleQuote);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
