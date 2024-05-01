using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Api;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Domain.Repositories;

namespace ApiApplication.Client.Http {
    internal class MoviesClientApi : IMoviesClientApi {
        public const string ClientName = "MoviesApiClient";
        public const int ExpirationTimeInMinutes = 30;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICacheRepository _cacheRepository;
        private readonly ILogger<MoviesClientApi> _logger;

        public MoviesClientApi(IHttpClientFactory httpClientFactory, ILogger<MoviesClientApi> logger, ICacheRepository cacheRepository) {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _cacheRepository = cacheRepository;
        }

        public async Task<MovieEntity> GetById(string id, CancellationToken token = default) {
            try {
                HttpClient httpClient = _httpClientFactory.CreateClient(ClientName);

                HttpResponseMessage response = await httpClient.GetAsync($"/v1/movies/{id}", token).ConfigureAwait(false);

                _ = response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                MovieDto dto = JsonConvert.DeserializeObject<MovieDto>(content);

                MovieEntity entity = dto?.ToEntity();

                await _cacheRepository.SetObjectInCache(id, entity, ExpirationTimeInMinutes, token);
                return entity;

            } catch (Exception ex) {
                _logger.LogInformation(ex.Message, ex);
                return await _cacheRepository.GetObjectFromCache<MovieEntity>(id, token);
            }
        }
    }
}
