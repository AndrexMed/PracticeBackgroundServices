using AutomaticProcess.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AutomaticProcess.Interfaces
{
    public interface ITestDbContext
    {
        DbSet<Persona> Personas { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}