using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ConnectSample.ViewModels;
using ConnectSample.Models;

namespace ConnectSample.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			if (Session["User"] == null)
				return RedirectToAction("Login");

			var user = Session["User"] as User;
			var tripDigestList = Trip.GetTripDigest(user);
			Session["TripDigestList"] = tripDigestList;

			return View(tripDigestList);


		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(User model, string returnUrl)
		{
			ViewBag.Message = "Login to your account";


			throw new Exception("Test error page.");

			//if (!model.Load())
			//{
			//	Session.Add("IsAuthenticated", "false");
			//	ViewData.Add(new KeyValuePair<string, object>("LoginError", "Login not found !!"));
			//	return View(model);
			//}

			//Session.Add("IsAuthenticated", "true");
			//Session.Add("User", model);
			//if (model.IsLinkedToConcur)
			//{
			//	Session.Add("IsLinkedToConcur", "true");
			//}

			//return RedirectToAction("Index");
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}


		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(User model, string returnUrl)
		{
			ViewBag.Message = "Register for a new account";

			model.Save();
			Session.Add("IsAuthenticated", "true");
			Session.Add("User", model);

			return RedirectToAction("Index");
		
		}

		[HttpGet]
		public ActionResult Link()
		{
			if (Session["User"] == null)
				return RedirectToAction("Login");

			var user = Session["User"] as User;
			user.Load();
			
			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Link(User model, string returnUrl)
		{
			ViewBag.Message = "Link to your Concur account";
			if (Session["User"] == null)
				return RedirectToAction("Login");

			model.LinkToConcur();
			model.SubscribeToChanges("profile");
			model.SubscribeToChanges("itinerary");

			model.Save();
			Session["User"] = model;

			if (model.IsLinkedToConcur)
				Session.Add("IsLinkedToConcur", "true");

			return View(model); 

		}

		[ValidateAntiForgeryToken]
		public ActionResult UnLink()
		{
			if (Session["User"] == null)
				return RedirectToAction("Login");

			User model = Session["User"] as User;
			model.UnLinkToConcur();
			Session.Add("IsLinkedToConcur", "false");

			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Manage()
		{
			if (Session["User"] == null)
				return RedirectToAction("Login");

			var model = Session["User"] as User;
			return View(model);
		}
	}
}
