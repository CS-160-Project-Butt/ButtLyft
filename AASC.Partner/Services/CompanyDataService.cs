using AASC.FW.Repositories;
using AASC.FW.Services;
using AASC.Partner.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Services
{
    public class CompanyDataService : Service<Company>, ICompanyDataService
    {
        protected readonly IRepositoryAsync<Company> _repository;

        public CompanyDataService(IRepositoryAsync<Company> repository)
            :base(repository)
        {
            _repository = repository;
        }
    }

    public interface ICompanyDataService : IService<Company>
    {

    }
}