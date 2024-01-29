using AutoMapper;
using Favourites.Domain.DTOs;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Infrastructure;
using Favourites.Application.Profiles;
using Moq;
using Serilog;
using Favourites.Application.Features.Commands.Handlers;

namespace Favourites.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected ILogger Logger;
        protected IBaseRepository<FavouritesHeader> HeaderRepository;
        protected IBaseRepository<FavouritesDetails> DetailsRepository;
        protected IBaseRepository<FavouritesDto> FavouroteRepository;

        public TestCommandHandler()
        {
            Context = FavouritesDbContextFactory.Create(FavouroteRepository);

            var headerRepository = new Mock<IBaseRepository<FavouritesHeader>>();
            var detailsRepository = new Mock<IBaseRepository<FavouritesDetails>>();
            var favouroteRepository = new Mock<IBaseRepository<FavouritesDto>>();

            Logger = Log.ForContext<FavoutiteUpsertRequestHandler>();

            HeaderRepository = headerRepository.Object;
            DetailsRepository = detailsRepository.Object;
            FavouroteRepository = favouroteRepository.Object;

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            FavouritesDbContextFactory.DestroyDatabase(Context);
        }
    }
}