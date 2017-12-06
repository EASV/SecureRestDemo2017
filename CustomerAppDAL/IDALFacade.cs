using System;
namespace CustomerAppDAL
{
    public interface IDALFacade
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
