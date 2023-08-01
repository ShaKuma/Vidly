using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Vidly.Models;
using Vidly.ViewModel;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        public ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Movies
        public ActionResult Index()
        {
            var movies = _context.Movie.Include(g => g.Genre).ToList();
            return View(movies);
        }

        public ActionResult MovieForm()
        {
            MovieFormViewModel viewModel = new MovieFormViewModel
            {
                Movie = new Movie(),
                Genre = _context.Genre.ToList()
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movie.SingleOrDefault(m => m.Id == id);
            if(movie is null)
            {
                return HttpNotFound();
            }
            else
            {
                MovieFormViewModel viewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    Genre = _context.Genre.ToList()
                };
                return View("MovieForm", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                MovieFormViewModel viewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    Genre = _context.Genre.ToList()
                };
                return View("MovieForm", viewModel);
            }
            if(movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movie.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movie.SingleOrDefault(m => m.Id == movie.Id);
                if(movieInDb is null)
                {
                    return HttpNotFound();
                }
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.StocksRemaining = movie.StocksRemaining;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}