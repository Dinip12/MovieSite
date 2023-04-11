using MovieSite.Entity;
using MovieSite.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using Scrypt;

namespace MovieSite.Repository
{
    public class UsersRepository
    {
        private AppDbContext context;

        public UsersRepository()
        {
            this.context = new AppDbContext();
        }
        public User GetById(int id)
        {
            return context.Users.Find(id);
        }
        public bool UserExisting(int id)
        {
            foreach (var item in context.Users)
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddUser(User item)
        {
            User user = new User();

            user.username = item.username;
            user.password = item.password;
            user.email = item.email;
            user.IsAdmin = item.IsAdmin;
            context.Users.Add(user);

            context.SaveChanges();
        }
        public void DeleteUser(int id)
        {
            
            context.Users.Remove(context.Users.Find(id));
            context.Ratings.RemoveRange(context.Ratings.Where(x => x.MovieId == id));
            context.Favorites.RemoveRange(context.Favorites.Where(x => x.MovieId == id));
            context.Comments.RemoveRange(context.Comments.Where(x => x.MovieId == id));
            context.SaveChanges();
        }
        public void UpdateUser(User item)
        {
            User user = context.Users.Find(item.Id);

            user.username = item.username;
            user.email = item.email;
            user.password = item.password;
            user.IsAdmin = item.IsAdmin;
            context.Entry(user).State = EntityState.Modified; //potencialno shte trqq se iztrie
            context.SaveChanges();
        }
        public List<User> GetAll(Expression<Func<User, bool>> filter = null, int page = 1, int pageSize = int.MaxValue)
        {
            IQueryable<User> query = context.Users;

            if (filter != null)
                query = query.Where(filter);

            return query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public User getByEmailAndPassword(string email, string password)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            foreach (var user in context.Users)
            {
                if (user.email==null|| user.password==null)
                {
                    return null;
                }
                if (user.email.Equals(email) && encoder.Compare(password, user.password))
                {
                    return user;
                }
            }
            return null;
        }
        public int UsersCount(Expression<Func<User, bool>> filter = null)
        {
            IQueryable<User> query = context.Users;

            if (filter != null)
                query = query.Where(filter);

            return query.Count();
        }
        public string GetUsernameById(int userid)
        {
            return context.Users.Where(x => x.Id == userid).FirstOrDefault().username;
        }
    }
}