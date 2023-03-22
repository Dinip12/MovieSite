using MovieSite.Entity;
using MovieSite.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using MovieSite.Repository;
public class RatingRepository
{
    private readonly AppDbContext context;
    private readonly MovieRepository movieRepository;
    public RatingRepository()
    {
        context = new AppDbContext();
        movieRepository = new MovieRepository();
    }
    public void isRated(Rating rating)
    {
        if (rating.Id > 0)
        {            
            UpdateRating(rating);
        }
        else
        {
            InsertRating(rating);
        }
    }
    public void InsertRating(Rating item)
    {
        Rating rating = new Rating();
        Movie movie = movieRepository.GetById(item.MovieId);

        rating.UserId = item.UserId;
        rating.MovieId = item.MovieId;
        rating.Rated = item.Rated;
        movie.Votes += 1;

        context.Entry(movie).State = EntityState.Modified;
        context.Ratings.Add(rating);
        context.SaveChanges();
    }

    public void UpdateRating(Rating item)
    {

        Rating rating = context.Ratings.Find(item.Id);

        rating.UserId = item.UserId;
        rating.MovieId = item.MovieId;
        rating.Rated = item.Rated;       

        context.Entry(rating).State = EntityState.Modified;
        context.SaveChanges();
    }
    public Rating FindRated(int movieid, int userId)
    {
        foreach (var item in context.Ratings)
        {
            if (item.MovieId == movieid && item.UserId == userId)
            {
                return item;
            }
        }
        return null;
    }
    public Rating GetById(int id)
    {
        return context.Ratings.Find(id);
    }
    public double GetAverageRating(int movieId)
    {
        try
        {
            return Math.Round(context.Ratings.Where(x => x.MovieId == movieId).Average(x => x.Rated), 2);
        }
        catch
        {

            return 0;
        }
       
    }
    /*Math.Round(context.Ratings.Where(x => x.MovieId == movieId).Average(x => x.Rated),2);*/
}

