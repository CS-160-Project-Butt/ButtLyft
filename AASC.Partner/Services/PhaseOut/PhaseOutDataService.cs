using AASC.FW.Repositories;
using AASC.FW.Services;
using AASC.Partner.API.Models;

namespace AASC.Partner.API.Services
{
    public class PhaseOutDataService : Service<PhaseOutPrep>, IPhaseOutDataService
    {
        protected readonly IRepositoryAsync<PhaseOutPrep> _repository;

        public PhaseOutDataService(IRepositoryAsync<PhaseOutPrep> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }

    public interface IPhaseOutDataService : IService<PhaseOutPrep>
    {

    }

}