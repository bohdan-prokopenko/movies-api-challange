using ApiApplication.Client.Configuration;
using ApiApplication.Domain.Api;
using ApiApplication.Domain.Entities;

using Grpc.Core;
using Grpc.Net.Client;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Client.Grpc {
    internal class ApiClientGrpc : IMoviesClientApi {
        private readonly IApiClientConfiguration _apiClientConfiguration;
        public ApiClientGrpc(IApiClientConfiguration apiClientConfiguration) {
            _apiClientConfiguration = apiClientConfiguration;
        }

        public async Task<MovieEntity> GetById(string movieId, CancellationToken token = default) {
            var httpHandler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel =
                GrpcChannel.ForAddress(_apiClientConfiguration.Address, new GrpcChannelOptions() {
                    HttpHandler = httpHandler
                });
            var client = new MoviesApi.MoviesApiClient(channel);
            var metadata = new Metadata {
                { "X-Apikey", _apiClientConfiguration.ApiKey }
            };
            responseModel all = await client.GetByIdAsync(new IdRequest { Id = movieId }, headers: metadata, cancellationToken: token);
            _ = all.Data.TryUnpack<showResponse>(out showResponse data);

            var success = int.TryParse(data.Year, out var year);
            var entity = new MovieEntity {
                ExternalId = data.Id,
                Title = data.Title,
                ImdbId = data.ImDbRating,
                Stars = data.Rank,
            };

            if (success) {
                entity.ReleaseDate = new DateTime(year, 1, 1);
            }

            return entity;
        }
    }
}