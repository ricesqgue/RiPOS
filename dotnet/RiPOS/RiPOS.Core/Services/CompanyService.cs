using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<CompanyResponse> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            var companyResponse = _mapper.Map<CompanyResponse>(company);

            return companyResponse;
        }
    }
}
