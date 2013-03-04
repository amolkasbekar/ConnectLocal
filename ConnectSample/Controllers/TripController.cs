using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ConnectSample.ViewModels;
using ConnectSample.Models;

namespace ConnectSample.Controllers
{
    public class TripController : Controller
    {
		public ActionResult Index(long Id)
		{
			if (Session["User"] == null)
				return RedirectToAction("Login", "Home");

			var user = Session["User"] as User;
			if (Session["TripDigestList"] == null)
				throw new ApplicationException("Error finding the trip specified. Please refresh and try again.");


			List<TripDigestViewModel> tripDigestList = Session["TripDigestList"] as List<TripDigestViewModel>;
			TripDigestViewModel tripSelected = tripDigestList.Where<TripDigestViewModel>(t => t.TripId == Id).FirstOrDefault();
			if (tripSelected == null)
				throw new ApplicationException("Error finding the trip specified. Please refresh and try again.");

			TripViewModel model = Trip.GetTrip(user, tripSelected);
			model.LoadDestinationData();

			return View(model);
		}

    }
}
