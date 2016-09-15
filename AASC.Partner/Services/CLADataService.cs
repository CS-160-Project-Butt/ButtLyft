using AASC.FW.Repositories;
using AASC.FW.Services;
using AASC.Partner.API.Models;

namespace AASC.Partner.API.Services
{
    public class CLADataService : Service<CLAForm>, ICLADataService
    {
        protected readonly IRepositoryAsync<CLAForm> _repository;

        public CLADataService(IRepositoryAsync<CLAForm> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }

    public interface ICLADataService : IService<CLAForm>
    {

    }

}