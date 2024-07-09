using AutomaticProcess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutomaticProcess.Services
{
    public class TestOneService(ITestRepository repository) : ITestOneService
    {
        private readonly ITestRepository _repository = repository;
        public async Task DisabledState()
        {
            await _repository.DisabledPerson();
        }
    }
}