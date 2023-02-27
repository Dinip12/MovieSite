﻿using Microsoft.AspNetCore.Mvc;
using MovieSite.Entity;
using MovieSite.Data;
using MovieSite.Repository;
using MovieSite.ViewModel.MovieVM;
using MovieSite.Data.Enums;
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
       
        public MovieController(IWebHostEnvironment webhost)
        {
            _webHostEnvironment = webhost;
            movieRepository = new MovieRepository();
            favoriteRepository = new FavoriteRepository();
            commentRepository = new CommentRepository();
            ratingRepository = new RatingRepository();
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult MovieAdmin(DisplayVM model)
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

            return View(model);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateVM item, IFormFile file)
        {
            MovieRepository movieRepo = new MovieRepository();
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

            movieRepo.InsertMovie(movie);
            return RedirectToAction("MovieAdmin", "Movie");
        }
        //------------------------------------------------------//
        //------------------DELETING Movie METHOD----------------//
        public IActionResult DeleteMovie(int id)
        {


            movieRepository.DeleteMovieByID(id);

            return RedirectToAction("MovieAdmin", "Movie");
        }


        //------------------------------------------------------//
        //------------------UPDATE Movie METHOD-------------------------//
        [HttpGet]
        public IActionResult UpdateMovie(int id)
        {
            AppDbContext context = new AppDbContext();
            Movie movie = movieRepository.GetById(id);// no idea if this is right
            EditVM item = new EditVM();

            item.ID = movie.Id;
            movie.Title = item.Title;
            movie.Description = item.Description;
            movie.movieCategory = item.MovieCategory;//should edit Movie.EditVM
            movie.TrailerURL = item.TrailerURL;
            return View(item);
        }
        [HttpPost]
        public IActionResult UpdateMovie(EditVM item)
        {
            MovieRepository movieRepo = new MovieRepository();
            Movie movie = new Movie();
            movie.Id = item.ID;
            movie.Title = item.Title;
            movie.Description = item.Description;
            movie.movieCategory = item.MovieCategory;//should edit Movie.EditVM
            movie.TrailerURL = item.TrailerURL;


            movieRepo.UpdateMovie(movie);

            return RedirectToAction("MovieAdmin", "Movie");//redirect to somewhere???
        }
        //------------------------------------------------------//
        public IActionResult Details(int id)
        {
            Movie item = movieRepository.GetById(id);

            DetailsVM movie = new DetailsVM();
            movie.id = item.Id;
            movie.Title = item.Title;
            movie.Description = item.Description;
            movie.MovieCategory = item.movieCategory;//should edit Movie.EditVM
            movie.TrailerURL = item.TrailerURL;
            movie.IsFavorite = item.IsFavorite;
            movie.comments = commentRepository.GetallCommentsByMovieId(movie.id);
            return View(movie);
        }
        
        public IActionResult Follow(int movieId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }
            int favorite = favoriteRepository.FindFavoriteId(movieId, Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            if (favorite > 0)
            {
                favoriteRepository.isFavorite(favorite, movieId);
            }
            else
            {
                int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
                favoriteRepository.isFavorite(UserId, movieId);
            }
            
             
            
            return RedirectToAction("Details", "Movie", new { id = movieId });
        }
        public IActionResult AddComment(int movieId,string text)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }
            Comment comment = new Comment();
            comment.MovieId = movieId;
            comment.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            comment.Text = text;
            UsersRepository userrepo = new UsersRepository();
            
            comment.Username = userrepo.GetUsernameById(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            commentRepository.InsertComment(comment);
            return RedirectToAction("Details", "Movie", new { id = movieId });
        }
        public IActionResult Rate(int movieId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }
            Rating rating = new Rating();
            Movie movie = movieRepository.GetById(movieId);

            movie.Votes++;

            ratingRepository.isRated(rating);
            movieRepository.UpdateMovie(movie);

            return RedirectToAction("Details", "Movie", new { id = movieId });
        }
    }
}