using ApiApplication.Domain.Entities;

using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    public interface IGetMovieUseCase {
        Task<MovieEntity> Execute(int movieId);
    }
}