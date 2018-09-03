using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Actmis.Models;

namespace Actmis.Controllers
{
    public class EventsController : Controller
    {
        ActmisDBEntities db = new ActmisDBEntities();

        // GET: Events

        //【Activity 主辦活動清單】
        public ActionResult Activity()
        {
            //取得登入會員的帳號並指定給fUserId
            string fUserId = (Session["Member"] as tMember).fUserId;
            ViewData["Message"] = fUserId;

            //找出未成為訂單明細的資料，即購物車內容
            var act = db.tEvents.Where(m => m.fUserId == fUserId).OrderByDescending(m => m.fDate).ThenBy(m => m.fPId).ToList();

            //指定ShoopingCar.cshtml套用_LayoutMember.cshtml，View使用orderDetails模型
            return View("Activity", "_LayoutMember", act);
        }

        //【Create 新增活動】
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string fPId, string fName, decimal fPrice, HttpPostedFileBase fImg, string fDescription, string fDate, string fType_no, string fArea, string fType_ca1, string fType_ca2, string fType_ca3, string fPosition, string fHost, string fPeople, string fDes_detail)
        {
            try
            {
                //上傳圖檔
                string fileName = "";
                //檔案上傳
                if (fImg != null)
                {
                    if (fImg.ContentLength > 0)
                    {
                        //取得圖檔名稱
                        fileName = System.IO.Path.GetFileName(fImg.FileName);
                        var path = System.IO.Path.Combine(Server.MapPath("~/Images"), fileName);
                        fImg.SaveAs(path); //檔案儲存到Images資料夾下
                    }
                }
                

                //取得登入會員的帳號並指定給fUserId
                string fUserId = (Session["Member"] as tMember).fUserId;
                //新增記錄
                tEvents act = new tEvents();
                act.fUserId = fUserId;
                act.fPId = fPId;
                act.fName = fName;
                act.fPrice = fPrice;
                act.fImg = fileName;
                act.fDescription = fDescription;
                act.fDate = fDate;
                act.fType_no = fType_no;
                act.fArea = fArea;
                act.fType_ca1 = fType_ca1;
                act.fType_ca2 = fType_ca2;
                act.fType_ca3 = fType_ca3;
                act.fPosition = fPosition;
                act.fHost = fHost;
                act.fPeople = fPeople;
                act.fDes_detail = fDes_detail;

                db.tEvents.Add(act);                    //物件加入act
                db.SaveChanges();
                return RedirectToAction("Activity");    //導向Activity的Action方法
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        //【Edit 編輯活動】
        public ActionResult Edit(int id)
        {
            var act = db.tEvents.Where(m => m.fId == id).FirstOrDefault();
            return View(act);
        }
        [HttpPost]
        public ActionResult Edit(int fId, string fPId, string fName, decimal fPrice, HttpPostedFileBase fImg, string fDescription, string fDate, string fType_no, string fArea, string fType_ca1, string fType_ca2, string fType_ca3, string fPosition, string fHost, string fPeople, string fDes_detail, string oldImg)
                                          
        {
            string fileName = "";
            //檔案上傳
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    //取得圖檔名稱
                    fileName = System.IO.Path.GetFileName(fImg.FileName);
                    var path = System.IO.Path.Combine(Server.MapPath("~/Images"), fileName);
                    fImg.SaveAs(path);
                }
            }
            else
            {
                fileName = oldImg; //若無上傳圖檔，則指定hidden隱藏欄位的資料
            }
            
            //取得登入會員的帳號並指定給fUserId
            string fUserId = (Session["Member"] as tMember).fUserId;
            // 修改資料
            var act = db.tEvents.Where(m => m.fPId == fPId).FirstOrDefault();
            act.fUserId = fUserId;
            act.fPId = fPId;
            act.fName = fName;
            act.fPrice = fPrice;
            act.fImg = fileName;
            act.fDescription = fDescription;
            act.fDate = fDate;
            act.fType_no = fType_no;
            act.fArea = fArea;
            act.fType_ca1 = fType_ca1;
            act.fType_ca2 = fType_ca2;
            act.fType_ca3 = fType_ca3;
            act.fPosition = fPosition;
            act.fHost = fHost;
            act.fPeople = fPeople;
            act.fDes_detail = fDes_detail;
            db.SaveChanges();
            return RedirectToAction("Activity");   //導向Activity的Action方法
        }

        //【Details 活動詳細】
        public ActionResult Details(int id)
        {
            var act = db.tEvents.Where(m => m.fId == id).FirstOrDefault();
            if (Session["Member"] == null)
            {
                return View("Details", "_Layout", act);
            }
            return View("Details", "_LayoutMember", act);
        }

        //【Delete 刪除活動】
        public ActionResult Delete(int id)
        {
            var act = db.tEvents.Where(m => m.fId == id).FirstOrDefault();
            string fileName = act.fImg; //取得要刪除產品的圖檔
            if (fileName != "")
            {
                //刪除指定圖檔
                System.IO.File.Delete(Server.MapPath("~/Images") + "/" + fileName);
            }
            db.tEvents.Remove(act);
            db.SaveChanges();
            return RedirectToAction("Activity");
        }

    }
}