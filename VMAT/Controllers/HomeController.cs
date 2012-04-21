using System.Collections.Generic;
using System.Web.Mvc;
using VMAT.Models;


namespace VMAT.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		private IConfigurationRepository configRepo;

		public HomeController() : 
			this(new ConfigurationRepository()) { }

		public HomeController(IConfigurationRepository config)
		{
			configRepo = config;
		}

		//
		// GET: /

		public ActionResult Index()
		{
			return RedirectToAction("Index", "VirtualMachine");
		}

		//
		// GET: /Help

		public ActionResult Help()
		{
			ViewBag.SupportEmail = AppConfiguration.GetSupportEmail();
			return View();
		}

		//
		// GET: /About

		public ActionResult About()
		{
			var companies = new List<Organization>();

			companies.Add(new Organization { 
				Name = "Rose-Hulman Institute of Technology",
				Authors = new List<string> { 
					"Nathan Mendel",
					"Anthony Sylvain",
					"Calvin Mlynarczyk",
					"Jacob Schmidt"
				},
				LogoFile = "logo_rhit.png"
			});

			companies.Add(new Organization {
				Name = AppConfiguration.GetClientCompany(),
				Authors = new List<string> {
					AppConfiguration.GetClientName()
				},
				LogoFile = "logo_gap.png"
			});

			return View(companies);
		}
	}
}
