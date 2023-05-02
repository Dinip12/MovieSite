using Microsoft.AspNetCore.Mvc;
using MovieSite.Data;
using MovieSite.Entity;
using MovieSite.Repository;
using MovieSite.ViewModel.MovieVM;
using MovieSite.ViewModel.Shared;
using System.Net.Mail;

namespace MovieSite.Controllers
{
	public class HomeController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private AppDbContext context;
		public HomeController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
			context = new AppDbContext();
		}
		public FileResult GetImage(int id)
		{

			Movie movies = context.Movies.Find(id);

			string rootPath = _webHostEnvironment.WebRootPath;
			var path = Path.Combine(rootPath + "\\img", movies.fileName + ".png");

			byte[] imageByteData = System.IO.File.ReadAllBytes(path);
			return File(imageByteData, "image/png");
		}
		public IActionResult Index(DisplayVM model)
		{

			model.Pager ??= new PagerVM();
			model.Filter ??= new FilterVM();
			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
										? 10
										: model.Pager.ItemsPerPage;

			model.Pager.Page = model.Pager.Page <= 0
									? 1
									: model.Pager.Page;

			var filter = model.Filter.GetFilter();

			MovieRepository movieRepository = new MovieRepository();
			model.Movies = movieRepository.GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage);
			model.Pager.PagesCount = (int)Math.Ceiling(movieRepository.MovieCount(filter) / (double)model.Pager.ItemsPerPage);
			return View(model);
		}
		[HttpGet]
		public IActionResult Contact()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Contact(string from, string name, string subject, string body)
		{
			MailProcessing smtp = new MailProcessing();
			smtp.SendEmailToSite(from, name, subject, body);
			return RedirectToAction("Contact", "Home");
		}
	}
}