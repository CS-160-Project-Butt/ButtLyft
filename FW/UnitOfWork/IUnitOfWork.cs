using AASC.FW.Infrastructure;
using AASC.FW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.FW.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : IObjectState;
        void BeginTransaction();
        bool Commit();
        void Rollback();
    }
}
