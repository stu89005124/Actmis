using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Actmis.Models;

namespace Cart.Controllers
{
    public class CartController : Controller
    {
        ActmisDBEntities db = new ActmisDBEntities();

        //【ShoppingCar 顯示購物車功能】
        public ActionResult ShoppingCar()
        {
            if (Session["Member"] == null)
            {
                return View("Index", "Home");
            }
            //取得登入會員的帳號並指定給fUserId
            string fUserId = (Session["Member"] as tMember).fUserId;
            ViewData["Message"] = fUserId;
            //找出未成為訂單明細的資料，即購物車內容
            var orderDetails = db.tOrderDetail.Where(m => m.fUserId == fUserId && m.fIsApproved == "否").ToList();
            //指定ShoopingCar.cshtml套用_LayoutMember.cshtml，View使用orderDetails模型
            return View("ShoppingCar", "_LayoutMember", orderDetails);
        }

        //【AddCar 加入購物車功能】
        public ActionResult AddCar(string fPId)
        {
            //取得會員帳號並指定給fUserId
            string fUserId = (Session["Member"] as tMember).fUserId;
            //找出會員放入訂單明細的產品，該產品的fIsApproved為"否"
            //表示該產品是購物車狀態
            var currentCar = db.tOrderDetail.Where(m => m.fPId == fPId && m.fIsApproved == "否" && m.fUserId == fUserId).FirstOrDefault();
            //若currentCar等於null，表示會員選購的產品不是購物車狀態
            if (currentCar == null)
            {
                //找出目前選購的產品並指定給product
                var product = db.tEvents.Where(m => m.fPId == fPId).FirstOrDefault();
                //將產品放入訂單明細，因為產品的fIsApproved為"否"，表示為購物車狀態
                tOrderDetail orderDetail = new tOrderDetail();
                orderDetail.fUserId = fUserId;
                orderDetail.fPId = product.fPId;
                orderDetail.fName = product.fName;
                orderDetail.fPrice = product.fPrice;
                orderDetail.fQty = 1;
                orderDetail.fIsApproved = "否";
                db.tOrderDetail.Add(orderDetail);
            }
            else
            {
                //若產品為購物車狀態，即將該產品數量加1
                currentCar.fQty += 1;
            }
            db.SaveChanges();
            //執行Home控制器的ShoppingCar動作方法
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("ShoppingCar");
        }
        public ActionResult BddCar(string fPId)
        {
            //取得會員帳號並指定給fUserId
            string fUserId = (Session["Member"] as tMember).fUserId;
            //找出會員放入訂單明細的產品，該產品的fIsApproved為"否"
            //表示該產品是購物車狀態
            var currentCar = db.tOrderDetail.Where(m => m.fPId == fPId && m.fIsApproved == "否" && m.fUserId == fUserId).FirstOrDefault();
            //若currentCar等於null，表示會員選購的產品不是購物車狀態
            if (currentCar == null)
            {
                //找出目前選購的產品並指定給product
                var product = db.tEvents.Where(m => m.fPId == fPId).FirstOrDefault();
                //將產品放入訂單明細，因為產品的fIsApproved為"否"，表示為購物車狀態
                tOrderDetail orderDetail = new tOrderDetail();
                orderDetail.fUserId = fUserId;
                orderDetail.fPId = product.fPId;
                orderDetail.fName = product.fName;
                orderDetail.fPrice = product.fPrice;
                orderDetail.fQty = 1;
                orderDetail.fIsApproved = "否";
                db.tOrderDetail.Add(orderDetail);
            }
            else
            {
                //若產品為購物車狀態，即將該產品數量加1
                currentCar.fQty += 1;
            }
            db.SaveChanges();
            //執行Home控制器的ShoppingCar動作方法
            return RedirectToAction("Filter", "Home");
            //return RedirectToAction("ShoppingCar");
        }
        //【DeleteCar 刪除購物車功能】
        public ActionResult DeleteCar(int fId)
        {
            // 依fId找出要刪除購物車狀態的產品
            var orderDetail = db.tOrderDetail.Where(m => m.fId == fId).FirstOrDefault();
            //刪除購物車狀態的產品
            db.tOrderDetail.Remove(orderDetail);
            db.SaveChanges();
            //執行Home控制器的ShoppingCar動作方法
            return RedirectToAction("ShoppingCar");
        }

        //Post:Index/ShoppingCar
        [HttpPost]
        public ActionResult ShoppingCar(string fReceiver, string fEmail, string fAddress)
        {
            //找出會員帳號並指定給fUserId
            string fUserId = (Session["Member"] as tMember).fUserId;
            //建立唯一的識別值並指定給guid變數，用來當做訂單編號
            //tOrder的fOrderGuid欄位會關聯到tOrderDetail的fOrderGuid欄位
            //形成一對多的關係，即一筆訂單資料會對應到多筆訂單明細
            string guid = Guid.NewGuid().ToString();
            //建立訂單主檔資料
            tOrder order = new tOrder();
            order.fOrderGuid = guid;
            order.fUserId = fUserId;
            order.fReceiver = fReceiver;
            order.fEmail = fEmail;
            order.fAddress = fAddress;
            order.fDate = DateTime.Now;
            db.tOrder.Add(order);
            //找出目前會員在訂單明細中是購物車狀態的產品
            var carList = db.tOrderDetail.Where(m => m.fIsApproved == "否" && m.fUserId == fUserId).ToList();
            //將購物車狀態產品的fIsApproved設為"是"，表示確認訂購產品
            foreach (var item in carList)
            {
                item.fOrderGuid = guid;
                item.fIsApproved = "是";
            }
            //更新資料庫，異動tOrder和tOrderDetail
            //完成訂單主檔和訂單明細的更新
            db.SaveChanges();
            //執行Home控制器的OrderList動作方法
            return RedirectToAction("OrderList", "Order");
        }
    }
}