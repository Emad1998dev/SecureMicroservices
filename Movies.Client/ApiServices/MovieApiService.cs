//using IdentityModel.Client;
//using Movies.Client.Models;
//using Newtonsoft.Json;

//namespace Movies.Client.ApiServices
//{
//    public class MovieApiService : IMovieApiService
//    {
//        private readonly List<Movie> _movies;
//        private readonly IHttpClientFactory _httpClientFactory;

//        // Constructor with dependency injection
//        public MovieApiService(IHttpClientFactory httpClientFactory)
//        {
//            _movies = new List<Movie>();
//            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
//        }


//        // Default constructor with dummy data
//        public MovieApiService()
//        {
//            // Initialize with some dummy data
//            _movies = new List<Movie>
//            {
//                new Movie
//                {
//                    Id = 1,
//                    Genre = "comics",
//                    Title = "Mr. Robot",
//                    Rateing = "9.9",
//                    ImageUrl = "images/src",
//                    ReleaseData = DateTime.Now,
//                    Owner = "Emad"
//                }
//            };
//        }

//        // Create a new movie and add it to the list
//        public Task<Movie> CreateMovie(Movie movie)
//        {
//            if (movie == null) throw new ArgumentNullException(nameof(movie));

//            movie.Id = _movies.Count > 0 ? _movies.Max(m => m.Id) + 1 : 1;
//            _movies.Add(movie);
//            return Task.FromResult(movie);
//        }

//        // Delete a movie by its ID
//        public Task<Movie> DeleteMovie(string id)
//        {
//            if (!int.TryParse(id, out int movieId)) throw new ArgumentException("Invalid ID format");

//            var movie = _movies.FirstOrDefault(m => m.Id == movieId);
//            if (movie == null) throw new KeyNotFoundException("Movie not found");

//            _movies.Remove(movie);
//            return Task.FromResult(movie);
//        }

//        // Retrieve a movie by its ID
//        public Task<Movie> GetMoviebyid(string id)
//        {
//            if (!int.TryParse(id, out int movieId)) throw new ArgumentException("Invalid ID format");

//            var movie = _movies.FirstOrDefault(m => m.Id == movieId);
//            if (movie == null) throw new KeyNotFoundException("Movie not found");

//            return Task.FromResult(movie);
//        }

//        // Retrieve all movies from the external API
//        public async Task<IEnumerable<Movie>> GetMovies()
//        {
//            //ایجاد کلاینت HTTP از IHttpClientFactory
//            //کلاینت HTTP از طریق فکتوری ایجاد می‌شود. این روش مدیریت بهتر اتصال‌ها و کارایی را فراهم می‌کند
//            var httpclient = _httpClientFactory.CreateClient("MovieAPIClient");

//            //ساخت شیء درخواست HTTP
//            //درخواست GET برای مسیر /api/movies ساخته می‌شود
//            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies");

//            //ارسال درخواست
//            //درخواست به API ارسال شده و پاسخ دریافت می‌شود. گزینه ResponseHeadersRead تضمین می‌کند که محتوا به‌صورت جریانی پردازش شود
//            var response = await httpclient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

//            //بررسی موفقیت پاسخ
//            //اگر وضعیت پاسخ موفقیت‌آمیز نبود، استثنا پرتاب می‌شود
//            response.EnsureSuccessStatusCode();

//            //خواندن محتوای پاسخ
//            //محتوای پاسخ به‌صورت متن JSON خوانده می‌شود
//            var content = await response.Content.ReadAsStringAsync();

//            //تبدیل JSON به لیست فیلم‌ها
//            //محتوای JSON به لیستی از اشیاء Movie تبدیل می‌شود. اگر null باشد، یک لیست خالی بازگردانده می‌شود.
//            var movieList = JsonConvert.DeserializeObject<List<Movie>>(content) ?? new List<Movie>();
//            return movieList;
//        }

//        // Update an existing movie
//        public Task<Movie> UpdateMovie(Movie movie)
//        {
//            if (movie == null) throw new ArgumentNullException(nameof(movie));

//            var existingMovie = _movies.FirstOrDefault(m => m.Id == movie.Id);
//            if (existingMovie == null) throw new KeyNotFoundException("Movie not found");

//            existingMovie.Title = movie.Title;
//            existingMovie.Genre = movie.Genre;
//            existingMovie.Rateing = movie.Rateing;
//            existingMovie.ImageUrl = movie.ImageUrl;
//            existingMovie.ReleaseData = movie.ReleaseData;
//            existingMovie.Owner = movie.Owner;

//            return Task.FromResult(existingMovie);
//        }
//    }
//}
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly List<Movie> _movies;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor with dependency injection
        public MovieApiService(IHttpClientFactory httpClientFactory , IHttpContextAccessor httpContextAccessor)
        {
            _movies = new List<Movie>();
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor;
        }

        // Default constructor with dummy data
        public MovieApiService()
        {
            _movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Genre = "comics",
                    Title = "Mr. Robot",
                    Rateing = "9.9",
                    ImageUrl = "images/src",
                    ReleaseData = DateTime.Now,
                    Owner = "Emad"
                }
            };
        }

        // Create a new movie and add it to the list
        public async Task<Movie> CreateMovie(Movie movie)
        {
            if (movie == null) throw new ArgumentNullException(nameof(movie));

            // If you are using external API, here would be an HTTP POST request to create the movie
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/movies")
            {
                Content = new StringContent(JsonConvert.SerializeObject(movie), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var createdMovie = JsonConvert.DeserializeObject<Movie>(await response.Content.ReadAsStringAsync());
            return createdMovie;
        }

        // Delete a movie by its ID
        public async Task<Movie> DeleteMovie(string id)
        {
            if (!int.TryParse(id, out int movieId)) throw new ArgumentException("Invalid ID format");

            // Delete movie via external API
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/movies/{movieId}");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return new Movie { Id = movieId }; // Assuming that movie is deleted, return the deleted movie details
        }

        // Retrieve a movie by its ID
        public async Task<Movie> GetMoviebyid(string id)
        {
            if (!int.TryParse(id, out int movieId)) throw new ArgumentException("Invalid ID format");

            // Get movie from the external API
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/movies/{movieId}");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var movie = JsonConvert.DeserializeObject<Movie>(await response.Content.ReadAsStringAsync());
            return movie;
        }

        // Retrieve all movies from the external API
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            // Create client and send request to get movies
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/movies");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Deserialize the response content to a list of movies
            var content = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(content) ?? new List<Movie>();
            return movieList;
        }

        // Update an existing movie
        public async Task<Movie> UpdateMovie(Movie movie)
        {
            if (movie == null) throw new ArgumentNullException(nameof(movie));

            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/movies/{movie.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(movie), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var updatedMovie = JsonConvert.DeserializeObject<Movie>(await response.Content.ReadAsStringAsync());
            return updatedMovie;
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpclient = _httpClientFactory.CreateClient("IDPclient");
            var metaDataResponse = await idpclient.GetDiscoveryDocumentAsync();
            if(metaDataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token ");
            }
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var userInfoResponse = await idpclient.GetUserInfoAsync(
                new UserInfoRequest
            { 
                    Address = metaDataResponse.UserInfoEndpoint,
                    Token = accessToken
            });

            if(userInfoResponse.IsError)
            {
                throw new HttpRequestException("somthing went wrong while getting user info");
            }
            var userInfoDictionary = new Dictionary<string, string>();
            foreach(var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }
    }
}
