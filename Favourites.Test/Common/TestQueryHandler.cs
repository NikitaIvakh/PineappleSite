using AutoMapper;
using Favourites.Application.Features.Commands.Queries;
using Favourites.Application.Profiles;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Domain.Interfaces.Services;
using Favourites.Infrastructure;
using Favourites.Infrastructure.Repositories;
using Moq;
using Serilog;
using Xunit;

namespace Favourites.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IBaseRepository<FavouritesHeader> HeaderRepository;
        protected IBaseRepository<FavouritesDetails> DetailsRepository;
        protected ILogger GetFavouriteListLogger;
        protected IMapper Mapper;
        protected IProductService ProductService;

        public TestQueryHandler()
        {
            var productMock = new Mock<IProductService>();

            Context = FavouritesDbContextFactory.Create();
            HeaderRepository = new BaseRepository<FavouritesHeader>(Context);
            DetailsRepository = new BaseRepository<FavouritesDetails>(Context);
            GetFavouriteListLogger = Log.ForContext<GetFavouriteProductsRequestHandler>();
            ProductService = productMock.Object;

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            FavouritesDbContextFactory.DestroyDatabase(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}