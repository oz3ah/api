using Shortha.Application.Interfaces.Services;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class ExtensionService(IExtensionRepository repo) : IExtensionService
{
}