using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MovieSite.Entity;
using MovieSite.Repository;
using MovieSite.ViewModel.Shared;
using MovieSite.ViewModel.UserVM;
using Scrypt;
using System.Security.Claims;

namespace ProjectManager.Controllers
{
    public class UsersController : Controller
    {
        private UsersRepository userRepo;
        private ScryptEncoder encoder;
        public UsersController()
        {
            this.userRepo = new UsersRepository();
            this.encoder = new ScryptEncoder();
        }
        //------------------LOGOUT-------------------------------//
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------//
        //------------------REGISTER SCREEN----------------------//
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(RegisterVM item)
        {
            if (!ModelState.IsValid)
                return View(item);


            User user = new User();

            user.username = item.Username;
            user.password = encoder.Encode(item.Password);
            user.email = item.Email;
            user.IsAdmin = false;
            userRepo.AddUser(user);
            List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier , $"{user.username}"),
                    new Claim(ClaimTypes.Email , user.email),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role , user.IsAdmin ? "Admin": "User"),
                    new Claim(ClaimTypes.Sid , user.Id.ToString())
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------//
        //------------------LOGIN SCREEN-------------------------//
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            User user = userRepo.getByEmailAndPassword(Email, Password);
            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier , $"{user.username}"),
                    new Claim(ClaimTypes.Email , user.email),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role , user.IsAdmin ? "Admin": "User"),
                    new Claim(ClaimTypes.Sid , user.Id.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
            }
            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------//
        //------------------DISPLAYS ALL USERS-------------------//
        public IActionResult UserList(IndexVM model)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
                return RedirectToAction("Index", "Home");
            else
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


                model.Items = userRepo.GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage);
                model.Pager.PagesCount = (int)Math.Ceiling(userRepo.UsersCount(filter) / (double)model.Pager.ItemsPerPage);

                return View(model);
            }
        }
        //-------------------------------------------------------//
        //------------------ADD METHOD---------------------------//
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateVM item)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
                return RedirectToAction("Index", "Home");
            else
            {
                if (!ModelState.IsValid)
                    return View(item);

                User user = new User();
                user.username = item.Username;
                user.password = encoder.Encode(item.Password);
                user.email = item.Email;
                user.IsAdmin = item.IsAdmin;

                userRepo.AddUser(user);
                return RedirectToAction("UserList", "Users");
            }
        }
        //------------------------------------------------------//
        //------------------DELETING USER METHOD----------------//
        public IActionResult DeleteUser(int id)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
                return RedirectToAction("Index", "Home");
            userRepo.DeleteUser(id);

            return RedirectToAction("UserList", "Users");
        }
        //------------------------------------------------------//
        //------------------UPDATE USER-------------------------//
        [HttpGet]
        public IActionResult UpdateUser(int id)
        {

            User user = userRepo.GetById(id);
            EditVM item = new EditVM();

            item.ID = user.Id;
            item.Username = user.username;
            item.Password = user.password;
            item.Email = user.email;
            item.IsAdmin = user.IsAdmin;

            return View(item);
        }
        [HttpPost]
        public IActionResult UpdateUser(EditVM item)
        {

            User user = new User();
            user.Id = item.ID;
            user.username = item.Username;
            user.password = encoder.Encode(item.Password);
            user.email = item.Email;
            user.IsAdmin = item.IsAdmin;


            userRepo.UpdateUser(user);

            return RedirectToAction("UserList", "Users");
        }
        //------------------------------------------------------//
        [HttpGet]
        public IActionResult UserSettings()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }

            User user = userRepo.GetById(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));

            EditVM edit = new EditVM
            {
                ID = user.Id,
                Username = user.username,
                Email = user.email,
                Password = ""
            };

            return View(edit);
        }
        [HttpPost]
        public IActionResult UserSettings(EditVM viewModel)
        {
            User user = userRepo.GetById(viewModel.ID);

            user.email = viewModel.Email;
            if (!string.IsNullOrEmpty(viewModel.Password))
            {
                user.password = encoder.Encode(viewModel.Password);
            }
            userRepo.UpdateUser(user);

            return RedirectToAction("UserSettings");
        }

    }
}
