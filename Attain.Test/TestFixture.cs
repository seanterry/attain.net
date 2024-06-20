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
    static readonly IServiceProvider rootServiceProvider;
    readonly IServiceScope scope = rootServiceProvider.CreateScope();

    protected IServiceProvider ServiceProvider => scope.ServiceProvider;
    protected AppDbContext Db => ServiceProvider.GetRequiredService<AppDbContext>();

    static TestFixture()
    {
        var host = Host.CreateApplicationBuilder();
        host.Configuration.AddUserSecrets<TestFixture>();

        host.AddAppDbContext();

        rootServiceProvider = host.Services.BuildServiceProvider();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Task.CompletedTask;

    public virtual void Dispose()
    {
        scope.Dispose();
        GC.SuppressFinalize( this );
    }
}
