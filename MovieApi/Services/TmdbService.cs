using MovieApi.DTOs.TMDB;

namespace MovieApi.Services
{
    public interface ITmdbService
    {
        // Películas
        Task<TmdbMovieListResponse?> GetPopularMoviesAsync(int page = 1, string language = "es-ES");
        Task<TmdbMovieListResponse?> GetTopRatedMoviesAsync(int page = 1, string language = "es-ES");
        Task<TmdbMovieListResponse?> GetNowPlayingMoviesAsync(int page = 1, string language = "es-ES");
        Task<TmdbMovieListResponse?> GetUpcomingMoviesAsync(int page = 1, string language = "es-ES");
        Task<TmdbMovieListResponse?> SearchMoviesAsync(string query, int page = 1, string language = "es-ES");
        Task<TmdbMovieDetails?> GetMovieDetailsAsync(int movieId, string language = "es-ES");
        
        // Series de TV
        Task<TmdbTvListResponse?> GetPopularTvShowsAsync(int page = 1, string language = "es-ES");
        Task<TmdbTvListResponse?> GetTopRatedTvShowsAsync(int page = 1, string language = "es-ES");
        Task<TmdbTvListResponse?> GetOnTheAirTvShowsAsync(int page = 1, string language = "es-ES");
        Task<TmdbTvListResponse?> SearchTvShowsAsync(string query, int page = 1, string language = "es-ES");
        Task<TmdbTvShowDetails?> GetTvShowDetailsAsync(int tvId, string language = "es-ES");
        Task<TmdbSeasonDetails?> GetSeasonDetailsAsync(int tvId, int seasonNumber, string language = "es-ES");
        
        // Búsqueda múltiple y tendencias
        Task<TmdbMultiSearchResponse?> MultiSearchAsync(string query, int page = 1, string language = "es-ES");
        Task<TmdbTrendingResponse?> GetTrendingAsync(string mediaType = "all", string timeWindow = "week", int page = 1, string language = "es-ES");
        
        // Géneros
        Task<TmdbGenreListResponse?> GetMovieGenresAsync(string language = "es-ES");
        Task<TmdbGenreListResponse?> GetTvGenresAsync(string language = "es-ES");
    }

    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TmdbService> _logger;
        private readonly string _baseUrl;
        private readonly string _bearerToken;
        private readonly string _imageBaseUrl;

        public TmdbService(HttpClient httpClient, IConfiguration configuration, ILogger<TmdbService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            _baseUrl = _configuration["TMDB:BaseUrl"] ?? "https://api.themoviedb.org/3";
            _bearerToken = _configuration["TMDB:BearerToken"] ?? "";
            _imageBaseUrl = _configuration["TMDB:ImageBaseUrl"] ?? "https://image.tmdb.org/t/p/w500";

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Películas

        public async Task<TmdbMovieListResponse?> GetPopularMoviesAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbMovieListResponse>($"/movie/popular?language={language}&page={page}");
        }

        public async Task<TmdbMovieListResponse?> GetTopRatedMoviesAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbMovieListResponse>($"/movie/top_rated?language={language}&page={page}");
        }

        public async Task<TmdbMovieListResponse?> GetNowPlayingMoviesAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbMovieListResponse>($"/movie/now_playing?language={language}&page={page}");
        }

        public async Task<TmdbMovieListResponse?> GetUpcomingMoviesAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbMovieListResponse>($"/movie/upcoming?language={language}&page={page}");
        }

        public async Task<TmdbMovieListResponse?> SearchMoviesAsync(string query, int page = 1, string language = "es-ES")
        {
            var encodedQuery = Uri.EscapeDataString(query);
            return await GetAsync<TmdbMovieListResponse>($"/search/movie?query={encodedQuery}&language={language}&page={page}");
        }

        public async Task<TmdbMovieDetails?> GetMovieDetailsAsync(int movieId, string language = "es-ES")
        {
            return await GetAsync<TmdbMovieDetails>($"/movie/{movieId}?language={language}&append_to_response=credits,videos");
        }

        #endregion

        #region Series de TV

        public async Task<TmdbTvListResponse?> GetPopularTvShowsAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbTvListResponse>($"/tv/popular?language={language}&page={page}");
        }

        public async Task<TmdbTvListResponse?> GetTopRatedTvShowsAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbTvListResponse>($"/tv/top_rated?language={language}&page={page}");
        }

        public async Task<TmdbTvListResponse?> GetOnTheAirTvShowsAsync(int page = 1, string language = "es-ES")
        {
            return await GetAsync<TmdbTvListResponse>($"/tv/on_the_air?language={language}&page={page}");
        }

        public async Task<TmdbTvListResponse?> SearchTvShowsAsync(string query, int page = 1, string language = "es-ES")
        {
            var encodedQuery = Uri.EscapeDataString(query);
            return await GetAsync<TmdbTvListResponse>($"/search/tv?query={encodedQuery}&language={language}&page={page}");
        }

        public async Task<TmdbTvShowDetails?> GetTvShowDetailsAsync(int tvId, string language = "es-ES")
        {
            return await GetAsync<TmdbTvShowDetails>($"/tv/{tvId}?language={language}&append_to_response=credits,videos");
        }

        public async Task<TmdbSeasonDetails?> GetSeasonDetailsAsync(int tvId, int seasonNumber, string language = "es-ES")
        {
            return await GetAsync<TmdbSeasonDetails>($"/tv/{tvId}/season/{seasonNumber}?language={language}");
        }

        #endregion

        #region Búsqueda múltiple y tendencias

        public async Task<TmdbMultiSearchResponse?> MultiSearchAsync(string query, int page = 1, string language = "es-ES")
        {
            var encodedQuery = Uri.EscapeDataString(query);
            return await GetAsync<TmdbMultiSearchResponse>($"/search/multi?query={encodedQuery}&language={language}&page={page}");
        }

        public async Task<TmdbTrendingResponse?> GetTrendingAsync(string mediaType = "all", string timeWindow = "week", int page = 1, string language = "es-ES")
        {
            // mediaType: all, movie, tv, person
            // timeWindow: day, week
            return await GetAsync<TmdbTrendingResponse>($"/trending/{mediaType}/{timeWindow}?language={language}&page={page}");
        }

        #endregion

        #region Géneros

        public async Task<TmdbGenreListResponse?> GetMovieGenresAsync(string language = "es-ES")
        {
            return await GetAsync<TmdbGenreListResponse>($"/genre/movie/list?language={language}");
        }

        public async Task<TmdbGenreListResponse?> GetTvGenresAsync(string language = "es-ES")
        {
            return await GetAsync<TmdbGenreListResponse>($"/genre/tv/list?language={language}");
        }

        #endregion

        #region Helper Methods

        private async Task<T?> GetAsync<T>(string endpoint) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                
                _logger.LogWarning("TMDB API request failed: {StatusCode} - {Endpoint}", 
                    response.StatusCode, endpoint);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling TMDB API: {Endpoint}", endpoint);
                return null;
            }
        }

        public string GetImageUrl(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            
            return $"{_imageBaseUrl}{path}";
        }

        #endregion
    }
}
