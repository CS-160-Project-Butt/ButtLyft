using AASC.FW.Repositories;
using AASC.FW.Services;
using AASC.Partner.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Services
{
    public class DepartmentDataService : Service<Department>, IDepartmentDataService
    {
        protected readonly IRepositoryAsync<Department> _repository;

        public DepartmentDataService(IRepositoryAsync<Department> repository)
            :base(repository)
        {
            _repository = repository;
        }
    }

    public interface IDepartmentDataService : IService<Department>
    {

    }
}