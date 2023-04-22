using Microsoft.AspNetCore.Mvc;
using MovieSite.Entity;
using MovieSite.Repository;
using MovieSite.ViewModel.MovieVM;
using MovieSite.ViewModel.Shared;
using System.Security.Claims;

namespace MovieSite.Controllers
{
	public class MovieController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly MovieRepository movieRepository;
		private readonly FavoriteRepository favoriteRepository;
		private readonly CommentRepository commentRepository;
		private readonly RatingRepository ratingRepository;
		private readonly UsersRepository userRepository;

		public MovieController(IWebHostEnvironment webhost)
		{
			_webHostEnvironment = webhost;
			movieRepository = new MovieRepository();
			favoriteRepository = new FavoriteRepository();
			commentRepository = new CommentRepository();
			ratingRepository = new RatingRepository();
			userRepository = new UsersRepository();
		}
		public IActionResult Categories(DisplayVM model)
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


			model.Movies = movieRepository.GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage);
			model.Pager.PagesCount = (int)Math.Ceiling(movieRepository.MovieCount(filter) / (double)model.Pager.ItemsPerPage);
			model.CurrentCategoryFilter = model.Filter.Category;
			return View(model);
		}
		public IActionResult MovieAdmin(DisplayVM model)
		{
			if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
				return RedirectToAction("Index", "Home");
			model.Pager ??= new PagerVM();
			model.Filter ??= new FilterVM();
			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
										? 10
										: model.Pager.ItemsPerPage;

			model.Pager.Page = model.Pager.Page <= 0
									? 1
									: model.Pager.Page;

			var filter = model.Filter.GetFilter();


			model.Movies = movieRepository.GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage);
			model.Pager.PagesCount = (int)Math.Ceiling(movieRepository.MovieCount(filter) / (double)model.Pager.ItemsPerPage);

			return View(model);
		}
		//------------------CREATING Movie METHOD---------------//
		[HttpGet]
		public IActionResult Create()
		{
			if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
				return RedirectToAction("Index", "Home");
			return View();
		}
		[HttpPost]
		public IActionResult Create(CreateVM item, IFormFile file)
		{

			Movie movie = new Movie();

			if (file != null)
			{
				movie.fileName = item.file.FileName.Substring(0, file.FileName.Length - 4);

				var path = Path.Combine(_webHostEnvironment.WebRootPath + "\\img", file.FileName);

				using var fileStream = new FileStream(path, FileMode.Create);
				file.CopyTo(fileStream);
			}

			movie.Title = item.Title;
			movie.Description = item.Description;
			movie.movieCategory = item.MovieCategory;
			movie.TrailerURL = item.TrailerURL;
			movie.Actors = item.Actors;
			movie.ReleaseYear = item.ReleaseYear;
			movie.Studio = item.Studio;

			movieRepository.InsertMovie(movie);
			return RedirectToAction("MovieAdmin", "Movie");
		}
		//------------------------------------------------------//
		//------------------DELETING Movie METHOD---------------//
		public IActionResult DeleteMovie(int id)
		{
			if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
				return RedirectToAction("Index", "Home");
			movieRepository.DeleteMovieByID(id);

			return RedirectToAction("MovieAdmin", "Movie");
		}
		//------------------------------------------------------//
		//------------------UPDATE Movie METHOD-----------------//
		[HttpGet]
		public IActionResult UpdateMovie(int id)
		{
			if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
				return RedirectToAction("Index", "Home");
			Movie movie = movieRepository.GetById(id);
			EditVM item = new EditVM();

			item.ID = movie.Id;
			movie.Title = item.Title;
			movie.Description = item.Description;
			movie.movieCategory = item.MovieCategory;
			movie.TrailerURL = item.TrailerURL;
			movie.Actors = item.Actors;
			movie.ReleaseYear = item.ReleaseYear;
			movie.Studio = item.Studio;

			return View(item);
		}
		[HttpPost]
		public IActionResult UpdateMovie(EditVM item)
		{

			Movie movie = new Movie();
			movie.Id = item.ID;
			movie.Title = item.Title;
			movie.Description = item.Description;
			movie.movieCategory = item.MovieCategory;
			movie.TrailerURL = item.TrailerURL;
			movie.Actors = item.Actors;
			movie.ReleaseYear = item.ReleaseYear;
			movie.Studio = item.Studio;

			movieRepository.UpdateMovie(movie);

			return RedirectToAction("MovieAdmin", "Movie");
		}
		//------------------------------------------------------//
		public IActionResult Details(int id)
		{
			Movie item = movieRepository.GetById(id);

			DetailsVM detailsVM = new DetailsVM();
			detailsVM.id = item.Id;
			detailsVM.Title = item.Title;
			detailsVM.Description = item.Description;
			detailsVM.MovieCategory = item.movieCategory;
			detailsVM.TrailerURL = item.TrailerURL;
			detailsVM.comments = commentRepository.GetallCommentsByMovieId(id);
			detailsVM.Rating = ratingRepository.GetAverageRating(id);
			detailsVM.Votes = item.Votes;
			detailsVM.Actors = item.Actors;
			detailsVM.ReleaseYear = item.ReleaseYear;
			detailsVM.Studio = item.Studio;
			try
			{
				detailsVM.favorite = favoriteRepository.FindFavorite(id, Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
			}
			catch
			{
				detailsVM.favorite = null;
			}
			return View(detailsVM);
		}
		public IActionResult AddComment(int movieId, string text)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Users");
			}
			Comment comment = new Comment();


			comment.MovieId = movieId;
			comment.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
			comment.Text = text;


			comment.Username = userRepository.GetUsernameById(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
			commentRepository.InsertComment(comment);
			return RedirectToAction("Details", "Movie", new { id = movieId });
		}
		//
		public IActionResult DeleteComment(int commentId, int movieId)
		{
			commentRepository.DeleteCommentByID(commentId);
			return RedirectToAction("Details", "Movie", new { id = movieId });
		}
		public ActionResult Rate(int movieId, int rated)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Users");
			}
			Rating rating = ratingRepository.FindRated(movieId, Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));

			if (rating == null)
			{
				rating = new Rating();
				rating.MovieId = movieId;
				rating.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
			}
			rating.Rated = rated;
			ratingRepository.isRated(rating);
			return RedirectToAction("Details", new { id = movieId });
		}
		public IActionResult Follow(int movieId)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Users");
			}
			Favorite favorite = favoriteRepository.FindFavorite(movieId, Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
			if (favorite == null)
			{
				favorite = new Favorite();
				favorite.MovieId = movieId;
				favorite.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
			}
			favoriteRepository.isFavorite(favorite);

			return RedirectToAction("Details", new { id = movieId });
		}
		public IActionResult FavoriteList(DisplayVM model)
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");
			model.Pager ??= new PagerVM();
			model.Filter ??= new FilterVM();
			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
										? 10
										: model.Pager.ItemsPerPage;

			model.Pager.Page = model.Pager.Page <= 0
										? 1
										: model.Pager.Page;

			model.Movies = favoriteRepository.getAllFavorites(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
			model.Pager.PagesCount = (int)Math.Ceiling(favoriteRepository.getAllFavorites(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)).Count / (double)model.Pager.ItemsPerPage);

			return View(model);
		}
	}
}