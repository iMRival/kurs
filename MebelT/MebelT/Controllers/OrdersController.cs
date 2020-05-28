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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace MebelT.Controllers
{
    public class OrdersController : Controller
    {
        private MebelEntities1 db = new MebelEntities1();

        // GET: Orders
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Index()
        {
            var orders = db.Orders.Include(o => o.Material).Include(o => o.Mebel);
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "admin, manager")]
        public ActionResult Create()
        {
            ViewBag.Material_id = new SelectList(db.Materials, "Material_id", "Material_name");
            ViewBag.Mebel_id = new SelectList(db.Mebels, "Mebel_id", "Mebel_name");
            if (db.Orders.Select(d => d.Mebel_id) == ViewBag.Mebel_id)
            {
                if (db.Orders.Select(d => d.Material_id) == ViewBag.Material_id)
                {
                    Order order = new Order { Price_for_one = ViewBag.Mebel_id + ViewBag.Material_id };
                }
            }
            return View();
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Create([Bind(Include = "Order_id,Order_date,Mebel_id,Material_id,Completion_date,Price_for_one,Amount,Total_price,Client")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Material_id = new SelectList(db.Materials, "Material_id", "Material_name", order.Material_id);
            ViewBag.Mebel_id = new SelectList(db.Mebels, "Mebel_id", "Mebel_name", order.Mebel_id);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Material_id = new SelectList(db.Materials, "Material_id", "Material_name", order.Material_id);
            ViewBag.Mebel_id = new SelectList(db.Mebels, "Mebel_id", "Mebel_name", order.Mebel_id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Edit([Bind(Include = "Order_id,Order_date,Mebel_id,Material_id,Completion_date,Price_for_one,Amount,Total_price,Client")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Material_id = new SelectList(db.Materials, "Material_id", "Material_name", order.Material_id);
            ViewBag.Mebel_id = new SelectList(db.Mebels, "Mebel_id", "Mebel_name", order.Mebel_id);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Report()
        {
            var orders = from Order in db.Orders select new { id = Order.Order_id, cost = Order.Total_price};
            if (orders == null)
            {
                return HttpNotFound();
            }
            var costs = from Order in db.Orders select Order.Total_price;
            if (costs == null)
            {
                return HttpNotFound();
            }
            var totalcost = costs.Sum();
            string reportname = @"C:\Users\hone\Desktop\report" + DateTime.Now.ToShortDateString() + ".pdf";
            iTextSharp.text.Document report = new iTextSharp.text.Document(PageSize.A4);//отчет
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            try
            {
                PdfWriter.GetInstance(report, new FileStream(reportname, FileMode.CreateNew));
                report.Open();
                foreach (var order in orders)
                {
                    report.Add(new Paragraph("Заказ №:" + order.id, font));
                    report.Add(new Paragraph("Общая стоимость:" + order.cost, font));
                    report.Add(new Paragraph(" ", font));
                }
                report.Add(new Paragraph("Стоимость всех заказов: " + totalcost, font));
                report.Close();
                report.Dispose();
                // отправка билета в принтер(в комментариях)
                //  PrintDocument printDoc = new PrintDocument();
                //printDoc.DocumentName = filename;
                //printDoc.Print();
                //printDoc.Dispose();
                // удаление билета, чтобы не занимал память, и еще не придумал как сохранять билеты с разными именами
                // System.IO.File.Delete(filename);
            }

            catch (Exception ex)
            {

            }
            finally
            {
                report.Close();
                report.Dispose();
            }
            return File(reportname, "MIME");
        }

        [HttpGet]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Cheque(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            string chequename = @"C:\Users\hone\Desktop\cheque" + DateTime.Now.ToShortDateString() + ".pdf";
            iTextSharp.text.Document cheque = new iTextSharp.text.Document(PageSize.A4);//отчет
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            try
            {
                PdfWriter.GetInstance(cheque, new FileStream(chequename, FileMode.CreateNew));
                cheque.Open();
                cheque.Add(new Paragraph("Номер заказа: " + order.Order_id, font));
                cheque.Add(new Paragraph("Наименование товара: " + order.Mebel.Mebel_name + " " + order.Material.Material_name, font));
                cheque.Add(new Paragraph("Цена товара: " + order.Price_for_one, font));
                cheque.Add(new Paragraph("Количество товара: " + order.Amount, font));
                cheque.Add(new Paragraph("Общая стоимость: " + order.Total_price, font));
                cheque.Add(new Paragraph("Клиент: " + order.Client, font));
                cheque.Close();
                cheque.Dispose();
                // отправка билета в принтер(в комментариях)
                //  PrintDocument printDoc = new PrintDocument();
                //printDoc.DocumentName = filename;
                //printDoc.Print();
                //printDoc.Dispose();
                // удаление билета, чтобы не занимал память, и еще не придумал как сохранять билеты с разными именами
                // System.IO.File.Delete(filename);
            }

            catch (Exception ex)
            {

            }
            finally
            {
                cheque.Close();
                cheque.Dispose();
            }
            return File(chequename, "MIME");
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
