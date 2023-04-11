using MovieSite.Entity;
using MovieSite.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
public class CommentRepository
{
    private readonly AppDbContext context;
    public CommentRepository()
    {
        context = new AppDbContext();
    }
    public void InsertComment(Comment item)
    {

        context.Comments.Add(item);
        context.SaveChanges();
    }
    public void DeleteCommentByID(int id)
    {


        context.Comments.Remove(context.Comments.Find(id));

        context.SaveChanges();
    }
    public void UpdateComment(Comment item)
    {

        Comment comment = context.Comments.Find(item.Id);

        comment.Id = item.Id;
        comment.UserId = item.UserId;
        comment.MovieId = item.MovieId;
        comment.Text = item.Text;
       

        context.Entry(comment).State = EntityState.Modified;
        context.SaveChanges();
    }
    public List<Comment> GetallCommentsByMovieId(int movieId)
    {
        return context.Comments.Where(x => x.MovieId == movieId).ToList();
    }
}

