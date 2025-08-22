using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories;

public class ExtensionRepository(DbContext context) : GenericRepository<Extension>(context), IExtensionRepository
{
}