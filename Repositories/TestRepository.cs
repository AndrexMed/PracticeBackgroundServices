using AutomaticProcess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutomaticProcess.Repositories
{
    public class TestRepository(ITestDbContext context) : ITestRepository
    {
        private readonly ITestDbContext _context = context;
        public async Task DisabledPerson()
        {
            var persons = await _context.Personas.ToListAsync();

            foreach (var item in persons)
            {
                item.EstadoRegistro = false;
            }
            await _context.SaveChangesAsync();
        }
    }
}
