using AASC.FW.Repositories;
using AASC.FW.Services;
using AASC.Partner.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Services
{
    public class EmployeeDataService : Service<Employee>, IEmployeeDataService
    {
        protected readonly IRepositoryAsync<Employee> _repository;

        public EmployeeDataService(IRepositoryAsync<Employee> repository)
            :base(repository)
        {
            _repository = repository;
        }
    }
}