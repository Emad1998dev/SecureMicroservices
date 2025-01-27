//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Protocols.OpenIdConnect;
//using Movies.Client.ApiServices;
//using Movies.Client.Models;
//using System.Diagnostics;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Movies.Client.Controllers
//{
//    [Authorize]
//    public class MoviesController : Controller
//    {
//        private readonly IMovieApiService _movieApiService;

//        public MoviesController(IMovieApiService movieApiService)
//        {
//            _movieApiService = movieApiService ?? throw new ArgumentNullException(nameof(movieApiService));
//        }

//        // GET: Movies
//        public async Task<IActionResult> Index()
//        {
//            await  LogTokenAndClaims();
//            var movies = await _movieApiService.GetMovies();
//            return View(movies);
//        }

//        public async Task LogTokenAndClaims()
//        {
//            var IdentityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
//            Debug.WriteLine($"IdentityToken:{IdentityToken}");
//            foreach(var claim in User.Claims)
//            {
//                Debug.WriteLine($"Claim type:{claim.Type} -Claim value:{claim.Value}");
//            }
//        }

//        // GET: Movies/Details/5
//        public async Task<IActionResult> Details(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return NotFound();
//            }

//            var movie = await _movieApiService.GetMoviebyid(id);
//            if (movie == null)
//            {
//                return NotFound();
//            }

//            return View(movie);
//        }

//        // GET: Movies/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Movies/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Rateing,ReleaseData,ImageUrl,Owner")] Movie movie)
//        {
//            if (ModelState.IsValid)
//            {
//                await _movieApiService.CreateMovie(movie);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(movie);
//        }

//        // GET: Movies/Edit/5
//        public async Task<IActionResult> Edit(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return NotFound();
//            }

//            var movie = await _movieApiService.GetMoviebyid(id);
//            if (movie == null)
//            {
//                return NotFound();
//            }

//            return View(movie);
//        }

//        // POST: Movies/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Genre,Rateing,ReleaseData,ImageUrl,Owner")] Movie movie)
//        {
//            if (id != movie.Id.ToString())
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                await _movieApiService.UpdateMovie(movie);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(movie);
//        }

//        // GET: Movies/Delete/5
//        public async Task<IActionResult> Delete(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return NotFound();
//            }

//            var movie = await _movieApiService.GetMoviebyid(id);
//            if (movie == null)
//            {
//                return NotFound();
//            }

//            return View(movie);
//        }

//        // POST: Movies/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(string id)
//        {
//            await _movieApiService.DeleteMovie(id);
//            return RedirectToAction(nameof(Index));
//        }


//        public async Task Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
//        }
//    }
//}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.ApiServices;
using Movies.Client.Models;
using System.Diagnostics;

namespace Movies.Client.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieApiService _movieApiService;

        // Constructor injection for the MovieApiService
        public MoviesController(IMovieApiService movieApiService)
        {
            _movieApiService = movieApiService ?? throw new ArgumentNullException(nameof(movieApiService));
        }

        public async Task<IActionResult> OnlyAdmin()
        {
            var UserInfo = await _movieApiService.GetUserInfo();
            return View(UserInfo);
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            await LogTokenAndClaims();
            var movies = await _movieApiService.GetMovies();
            return View(movies);
        }

        public async Task LogTokenAndClaims()
        {
            var IdentityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            Debug.WriteLine($"IdentityToken:{IdentityToken}");
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type:{claim.Type} -Claim value:{claim.Value}");
            }
        }

        // Action to show details of a specific movie
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var movie = await _movieApiService.GetMoviebyid(id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // Action to show the Create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Action to handle form submission for creating a movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
                return View(movie);

            await _movieApiService.CreateMovie(movie);
            return RedirectToAction(nameof(Index));
        }

        // Action to show the Edit form
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var movie = await _movieApiService.GetMoviebyid(id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // Action to handle form submission for editing a movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Movie movie)
        {
            if (id != movie.Id.ToString())
                return BadRequest();

            if (!ModelState.IsValid)
                return View(movie);

            await _movieApiService.UpdateMovie(movie);
            return RedirectToAction(nameof(Index));
        }

        // Action to show the Delete confirmation page
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var movie = await _movieApiService.GetMoviebyid(id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // Action to handle movie deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            await _movieApiService.DeleteMovie(id);
            return RedirectToAction(nameof(Index));
        }

        
       


        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}

