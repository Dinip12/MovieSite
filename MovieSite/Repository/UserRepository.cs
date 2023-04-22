using MovieSite.Data;
using MovieSite.Entity;
using Scrypt;
using System.Data.Entity;
using System.Linq.Expressions;

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
			context.Entry(user).State = EntityState.Modified;
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
			if (email == null || password == null)
				{
					return null;
				}
			User item = context.Users.Where(x => x.email == email).First();
			if (encoder.Compare(password, item.password))
			{
				return item;
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