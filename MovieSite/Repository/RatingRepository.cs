using MovieSite.Entity;
using MovieSite.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
public class RatingRepository
{
    private readonly AppDbContext context;
    public RatingRepository()
    {
        context = new AppDbContext();
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

        rating.UserId = item.UserId;
        rating.MovieId = item.MovieId;
        rating.Rated = item.Rated;
        context.Ratings.Add(rating);
        context.SaveChanges();
    }

    public void UpdateRating(Rating item)
    {

        Rating rating = context.Ratings.Find(item.Id);

        rating.UserId = item.UserId;
        rating.MovieId = item.MovieId;
        rating.Rated = item.Rated;
        rating.Votes = item.Votes;

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
}

