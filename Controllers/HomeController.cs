using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GooglereCAPTCHa.Models;
using Newtonsoft.Json;
using System.Web.Security;
using WebMatrix.WebData;

namespace GooglereCAPTCHa.Controllers
{
    [Serializable()]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            int loginCount = (int)Session["LoginCount"];
            if(loginCount>(int)Session["MaxLoginAttempts"])
            {
                ViewBag.ShowCaptcha = true;
                ViewBag.Public_key = "6Le1LR0TAAAAABJAs5x4ltxzpy0B8fM5PiBP-eQ5";
            }
                           
            return View();
        }

        [HttpPost]
        public ActionResult Index(CaptchaResponse captcha,string enableCaptcha)
        {

            int loginCount = (int)Session["LoginCount"];
            if(loginCount == (int)Session["MaxLoginAttempts"])
            {
                 ViewBag.Public_key = "6Le1LR0TAAAAABJAs5x4ltxzpy0B8fM5PiBP-eQ5";
                var response = Request["g-recaptcha-response"];
                ViewBag.ShowCaptcha = true;
            }
            if (loginCount > (int)Session["MaxLoginAttempts"])
            {
                ViewBag.Public_key = "6Le1LR0TAAAAABJAs5x4ltxzpy0B8fM5PiBP-eQ5";
                var response = Request["g-recaptcha-response"];
                ViewBag.ShowCaptcha = true;

                const string secret = "6Le1LR0TAAAAAOyvQ6D3zv9Mr5-6t2AoEGCITTkr";
                var client = new WebClient();
                var reply =
                    client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                //when response is false check for the error message
                if (!captchaResponse.Success)
                {
                    if (captchaResponse.ErrorCodes != null)
                    {
                        if (captchaResponse.ErrorCodes.Count <= 0) return View();

                        var error = captchaResponse.ErrorCodes[0].ToLower();
                        switch (error)
                        {
                            case ("missing-input-secret"):
                                ViewBag.Message = "The secret parameter is missing.";
                                break;
                            case ("invalid-input-secret"):
                                ViewBag.Message = "The secret parameter is invalid or malformed.";
                                break;

                            case ("missing-input-response"):
                                ViewBag.Message = "The response parameter is missing.";
                                break;
                            case ("invalid-input-response"):
                                ViewBag.Message = "The response parameter is invalid or malformed.";
                                break;

                            default:
                                ViewBag.Message = "Error occured. Please try again";
                                break;
                        }
                    }

                }
                else
                {
                    ViewBag.Message = "The captcha is valid";
                }

                //secret that was generated in key value pair
           
            }

            int logCount = (int)Session["LoginCount"];
            Session["LoginCount"] = logCount + 1;


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}