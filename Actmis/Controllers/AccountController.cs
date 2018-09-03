using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Actmis.Models;
using ezapp;

namespace Actmis.Controllers
{
    public class AccountController : Controller
    {
        ActmisDBEntities db = new ActmisDBEntities();
        
        // GET: Account

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string fUserId, string fPwd)
        {
            //MD5登入加密
            Cryptography cryp = new Cryptography();
            fPwd = cryp.SHA256Encode(fPwd);

            // 依帳密取得會員並指定給member
            var member = db.tMember.Where(m => m.fUserId == fUserId && m.fPwd == fPwd).FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                ViewBag.Msg = "帳密錯誤，登入失敗";
                return View();
            }
            //使用Session變數記錄歡迎詞
            Session["WelCome"] = member.fName + " 歡迎光臨！";
            //使用Session變數記錄登入的會員物件
            Session["Member"] = member;
            //執行Home控制器的Index動作方法
            return RedirectToAction("Index","Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        //Post:Home/Register
        [HttpPost]
        public ActionResult Register(tMember pMember,FormCollection post)
        {
            string fPwd = post["fPwd"].ToString();
            string fPwd2 = post["fPwd2"].ToString();
            //ViewBag.message2 = pMember.fName;
            //若模型沒有通過驗證則顯示目前的View
            if (ModelState.IsValid == false)
            {
                return View();
            }
            // 依帳號取得會員並指定給member
            var member = db.tMember
                .Where(m => m.fUserId == pMember.fUserId)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                if (string.IsNullOrWhiteSpace(fPwd) || fPwd != fPwd2) {
                    ViewBag.Message = "密碼與確認密碼不一致";
                    return View();
                }

                //MD5註冊加密
                Cryptography cryp = new Cryptography();
                pMember.fPwd = cryp.SHA256Encode(pMember.fPwd);

                db.tMember.Add(pMember);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            else{
                    ViewBag.Message = "此帳號已註冊過";
                    return View();
            }
            //將會員記錄新增到tMember資料表
            
            //執行Home控制器的Login動作方法   
           
        }
        public ActionResult Logout()
        {
            Session.Clear();  //清除Session變數資料
            return RedirectToAction("Index","Home");
        }
    }
}