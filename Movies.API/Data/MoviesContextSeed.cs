using Movies.API.Model;

namespace Movies.API.Data
{
    public class MoviesContextSeed
    {
        public static void SeedAsync(MoviesAPIContext moviesContext)
        {
            if (!moviesContext.Movie.Any())
            {
                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Id = 1,
                        Genre = "Drama",
                        Title = "The Shawshank Redemption",
                        Rateing = "9.3",
                        ImageUrl = "images/src",
                        ReleaseData = new DateTime(1994, 5, 5),
                        Owner = "alic"
                    },
                    new Movie
                    {
                        Id = 2,
                        Genre = "Crime",
                        Title = "The Godfather",
                        Rateing = "9.2",
                        ImageUrl = "images/the-godfather.jpg",
                        ReleaseData = new DateTime(1972, 3, 24),
                        Owner = "bobc"
                    },
                    new Movie
                    {
                        Id = 3,
                        Genre = "Action",
                        Title = "The Dark Knight",
                        Rateing = "9.0",
                        ImageUrl = "images/the-dark-knight.jpg",
                        ReleaseData = new DateTime(2008, 7, 18),
                        Owner = "carld"
                    },
                    new Movie
                    {
                        Id = 4,
                        Genre = "Drama",
                        Title = "12 Angry Men",
                        Rateing = "9.0",
                        ImageUrl = "images/12-angry-men.jpg",
                        ReleaseData = new DateTime(1957, 4, 10),
                        Owner = "davee"
                    },
                    new Movie
                    {
                        Id = 5,
                        Genre = "Sci-Fi",
                        Title = "Inception",
                        Rateing = "8.8",
                        ImageUrl = "images/inception.jpg",
                        ReleaseData = new DateTime(2010, 7, 16),
                        Owner = "frankf"
                    },
                    new Movie
                    {
                        Id = 6,
                        Genre = "Adventure",
                        Title = "The Lord of the Rings: The Return of the King",
                        Rateing = "8.9",
                        ImageUrl = "images/lotr-return.jpg",
                        ReleaseData = new DateTime(2003, 12, 17),
                        Owner = "georgeg"
                    },
                    new Movie
                    {
                        Id = 7,
                        Genre = "Biography",
                        Title = "Schindler's List",
                        Rateing = "8.9",
                        ImageUrl = "images/schindlers-list.jpg",
                        ReleaseData = new DateTime(1993, 11, 30),
                        Owner = "henryh"
                    },
                    new Movie
                    {
                        Id = 8,
                        Genre = "Animation",
                        Title = "Spirited Away",
                        Rateing = "8.6",
                        ImageUrl = "images/spirited-away.jpg",
                        ReleaseData = new DateTime(2001, 7, 20),
                        Owner = "irenei"
                    },
                    new Movie
                    {
                        Id = 9,
                        Genre = "Comedy",
                        Title = "Forrest Gump",
                        Rateing = "8.8",
                        ImageUrl = "images/forrest-gump.jpg",
                        ReleaseData = new DateTime(1994, 7, 6),
                        Owner = "jackj"
                    },
                    new Movie
                    {
                        Id = 10,
                        Genre = "Horror",
                        Title = "The Shining",
                        Rateing = "8.4",
                        ImageUrl = "images/the-shining.jpg",
                        ReleaseData = new DateTime(1980, 5, 23),
                        Owner = "katek"
                    }
                };

                // اضافه کردن داده‌ها به پایگاه داده
                moviesContext.Movie.AddRange(movies);
                moviesContext.SaveChanges();
            }
        }
    }
}
