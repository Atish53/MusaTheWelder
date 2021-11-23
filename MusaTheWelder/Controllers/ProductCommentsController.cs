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

namespace MusaTheWelder.Controllers
{
    public class ProductCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductComments
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductComments.ToListAsync());
        }

        // GET: ProductComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductComments productComments = await db.ProductComments.FindAsync(id);
            if (productComments == null)
            {
                return HttpNotFound();
            }
            return View(productComments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var productId = form["ProductId"];
            var rating = int.Parse(form["Rating"]);

            ProductComments productComment = new ProductComments()
            {
                ProductId = Convert.ToInt32(productId),
                Comments = comment,
                Rating = rating,
                Name = User.Identity.GetName(),
                ThisDateTime = DateTime.Now
            };

            db.ProductComments.Add(productComment);
            db.SaveChanges();

            return RedirectToAction("Details", "Products", new { id = productId });
        }

        // GET: ProductComments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentId,Comments,ThisDateTime,Name,ProductId,Rating")] ProductComments productComments)
        {
            if (ModelState.IsValid)
            {
                db.ProductComments.Add(productComments);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productComments);
        }

        // GET: ProductComments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductComments productComments = await db.ProductComments.FindAsync(id);
            if (productComments == null)
            {
                return HttpNotFound();
            }
            return View(productComments);
        }

        // POST: ProductComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentId,Comments,ThisDateTime,Name,ProductId,Rating")] ProductComments productComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productComments).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productComments);
        }

        // GET: ProductComments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductComments productComments = await db.ProductComments.FindAsync(id);
            if (productComments == null)
            {
                return HttpNotFound();
            }
            return View(productComments);
        }

        // POST: ProductComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductComments productComments = await db.ProductComments.FindAsync(id);
            db.ProductComments.Remove(productComments);
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
