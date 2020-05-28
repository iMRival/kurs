using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MebelT;

namespace MebelT.Controllers
{
    public class MebelsController : Controller
    {
        private MebelEntities1 db = new MebelEntities1();

        // GET: Mebels
        public async Task<ActionResult> Index()
        {
            return View(await db.Mebels.ToListAsync());
        }

        // GET: Mebels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mebel mebel = await db.Mebels.FindAsync(id);
            if (mebel == null)
            {
                return HttpNotFound();
            }
            return View(mebel);
        }

        // GET: Mebels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mebels/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Mebel_id,Mebel_name")] Mebel mebel)
        {
            if (ModelState.IsValid)
            {
                db.Mebels.Add(mebel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mebel);
        }

        // GET: Mebels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mebel mebel = await db.Mebels.FindAsync(id);
            if (mebel == null)
            {
                return HttpNotFound();
            }
            return View(mebel);
        }

        // POST: Mebels/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Mebel_id,Mebel_name")] Mebel mebel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mebel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mebel);
        }

        // GET: Mebels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mebel mebel = await db.Mebels.FindAsync(id);
            if (mebel == null)
            {
                return HttpNotFound();
            }
            return View(mebel);
        }

        // POST: Mebels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Mebel mebel = await db.Mebels.FindAsync(id);
            db.Mebels.Remove(mebel);
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
