using AASC.FW.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AASC.FW.Repositories
{
    public interface IRepositoryAsync<T> : IRepository<T> where T : IObjectState
    {
        Task<T> FindAsync(params object[] keyValues);
        Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
    }
}
