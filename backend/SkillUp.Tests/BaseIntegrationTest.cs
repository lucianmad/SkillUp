using Microsoft.Extensions.DependencyInjection;
using SkillUp.API.Database;

namespace SkillUp.Tests;

public class BaseIntegrationTest: IClassFixture<SkillUpWebApplicationFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly SkillUpWebApplicationFactory _factory;
    protected readonly HttpClient Client;
    protected readonly AppDbContext DbContext;

    protected BaseIntegrationTest(SkillUpWebApplicationFactory factory)
    {
        _factory = factory;
        Client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
    
    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }
}