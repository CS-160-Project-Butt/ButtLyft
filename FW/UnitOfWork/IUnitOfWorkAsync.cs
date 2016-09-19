using AASC.FW.Infrastructure;
using AASC.FW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AASC.FW.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<T> RepositoryAsync<T>() where T : IObjectState;
    }
}
