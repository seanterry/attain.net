// SPDX-License-Identifier: Proprietary
// Copyright (c) TTCO Holding Company, Inc. and Contributors
// All Rights Reserved.

using Attain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Attain;

public abstract class TestFixture : IDisposable, IAsyncLifetime
{
    static bool initialized;
    static readonly SemaphoreSlim semaphore = new( 1, 1 );
    static readonly IServiceProvider rootServiceProvider;
    readonly IServiceScope scope = rootServiceProvider.CreateScope();

    IServiceProvider ServiceProvider => scope.ServiceProvider;
    protected AppDbContext Db => ServiceProvider.GetRequiredService<AppDbContext>();

    static TestFixture()
    {
        var host = Host.CreateApplicationBuilder();
        host.Configuration.AddUserSecrets<TestFixture>();

        host.AddAppDbContext();

        rootServiceProvider = host.Services.BuildServiceProvider();
    }

    public virtual async Task InitializeAsync()
    {
        if ( initialized ) return;

        await semaphore.WaitAsync();

        try
        {
            if ( !initialized )
            {
                await Db.Database.EnsureDeletedAsync();
                await Db.Database.EnsureCreatedAsync();
                initialized = true;
            }
        }
        finally
        {
            semaphore.Release();
        }
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;

    public virtual void Dispose()
    {
        scope.Dispose();
        GC.SuppressFinalize( this );
    }
}
