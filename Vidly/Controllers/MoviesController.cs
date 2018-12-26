using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Movies
        public ViewResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre);
            return View(movies);
        }



        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return HttpNotFound();

            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }

        //        public ActionResult Random()
        //        {
        //            var movie = new Movie() {Name = "Shrek!"};
        //
        //            var customers = new List<Customer>
        //            {
        //                new Customer{ Name = "Customer 1" },
        //                new Customer{ Name = "Customer 2" }
        //            };
        //            var viewModel = new RandomMovieViewModel
        //            {
        //                Movie = movie,
        //                Customers = customers
        //            };
        //            return View(viewModel);
        //        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);
            }

            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {

                var movieInDb = _context.Movies.Single(c => c.Id == movie.Id);
//                TryUpdateModel(movieInDb, new string[]{"Name", "Genre", "ReleaseDay", "NumberInStock" , "DateAdded" });
//                TryUpdateModel(movieInDb);
                movieInDb.Name = movie.Name;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.ReleaseDay = movie.ReleaseDay;
                movieInDb.NumberInStock = movie.NumberInStock;

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

        public ViewResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }
    }
}