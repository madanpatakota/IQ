using Misard.IQs.Domain.Entities;

namespace Misard.IQs.Application.Interfaces.Repositories;

public interface ITechnologyRepository
{
    Task<List<Technology>> GetAllAsync();
}
