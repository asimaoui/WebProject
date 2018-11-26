using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebScrach2.Models;

namespace WebScrach2.Controllers
{
    public class HomeController : Controller
    {
        amugh786_aziz_dbEntities db = new amugh786_aziz_dbEntities();

         [AllowAnonymous]
        // GET: Home
        public ActionResult Index(string searchString, int? dropDownID=0)
        {

            ViewBag.CatelogComponent = new SelectList(db.CatelogComponents.OrderBy(x=>x.Name), "ComponentID", "Name");

            var movies = db.Movies.ToList();

            double retNum = -1;
            bool isNum = Double.TryParse(Convert.ToString(searchString), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);


            switch (dropDownID)
            {
                case 1://Discription           
                    movies = movies.Where(s => s.Description.Contains(searchString)).ToList();
                    break;
                case 2://Platform
                    var listLinkP = from p in db.MovieLinkPlatforms join m in  db.Plateforms on p.PlatformID  equals m.PlateformID where m.Name.Contains(searchString)  select p;
                    var selectedMovies = from m in movies join ml in listLinkP on m.MovieID equals ml.MovieID select m;
                    movies = selectedMovies.ToList();
                    break;
                case 3://Rating
                    //movies = movies.Where(s => s.Rating.Equals(searchString)).ToList();
                    if (isNum)
                    {
                        var mm = from p in movies where p.Rating == (retNum) select p;
                        movies = mm.ToList();
                    }
                    else
                    {
                        movies = defaultSearch(movies, searchString);
                    }

                    break;
                case 4://Genre
                    var listLinkGenre = from p in db.MovieLinkGenres
                                    join m in  db.Genres on p.GenreID equals m.id where m.Category.Contains(searchString)  select p  ;
                    var selectedMov = from m in movies
                                    join ml in listLinkGenre on m.MovieID equals ml.MovieID
                                    select m;
                    movies = selectedMov.ToList();
                    break;
                case 5://Price
                    if (isNum)
                    {
                        var mm = from p in movies where p.Price == (retNum) select p;
                        movies = mm.ToList();
                    }
                    else
                    {
                        movies = defaultSearch(movies, searchString);
                    }
                    break;
                case 6://Title
                    movies = movies.Where(s => s.Title.Contains(searchString)).ToList();
                    break;
                case 0:
                    movies = defaultSearch(movies, searchString);
                    break;
                default:

                    break;
            }

            var movhome = new MovieHome
            {
                movis = movies,
                searchString = searchString,
                dropdownID = dropDownID.Value
            };

            //return View(db.Movies.ToList());

            return View(movhome);
        }

        private List<Movy> defaultSearch(List<Movy> movies, string searchString){
            if (searchString != null)
            {
                double retNum = -1;
                bool isNum = Double.TryParse(Convert.ToString(searchString), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

                movies = movies.Where(s => s.Description.Contains(searchString) |
                    s.Title.Contains(searchString) |
                    s.Price.Equals(retNum) |
                    s.Rating.Equals(retNum) |
                    s.Title.Contains(searchString)
                    ).ToList();

                 

                var lLP = from p in db.MovieLinkPlatforms join m in db.Plateforms on p.PlatformID equals m.PlateformID where m.Name.Contains(searchString) select p;
                var sM = from m in movies join ml in lLP on m.MovieID equals ml.MovieID select m;
                movies.Concat(sM.ToList());

                var lLG = from p in db.MovieLinkGenres join m in db.Genres on p.GenreID equals m.id where m.Category.Contains(searchString) select p;
                var slM = from m in movies join ml in lLG on m.MovieID equals ml.MovieID select m;
                movies.Concat(slM.ToList());

            }

            return movies;
        }

        [HttpPost]
        public ActionResult Index(MovieHome model)
        {

            return View(model);
        }


    }
}