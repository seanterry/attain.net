// SPDX-License-Identifier: Proprietary
// Copyright (c) TTCO Holding Company, Inc. and Contributors
// All Rights Reserved.

using Amazon;
using Attain.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Attain;

/// <summary>
/// Extensions methods for configuring applications.
/// </summary>
public static class StartupExtensions
{
    public static void AddAppDbContext( this IHostApplicationBuilder builder )
    {
        var sourceBuilder = new NpgsqlDataSourceBuilder( builder.Configuration.GetConnectionString( "attain" ) );
        sourceBuilder.UseNodaTime();

        var source = sourceBuilder.Build();

        builder.Services.AddDbContext<AppDbContext>( options =>
        {
            options.UseNpgsql( source, pg =>
            {
                pg.UseNodaTime();
                pg.SetPostgresVersion( 12, 16 );
                pg.MigrationsHistoryTable( "__ef_migrations_history", "my" );
            } );
        } );
    }

    public static void AddSystemsManagerSecrets( this IHostApplicationBuilder builder )
    {
        AWSConfigs.AWSRegion = RegionEndpoint.USWest2.SystemName;

        builder.Configuration
            .AddSystemsManager( "/attain/my/all/" )
            .AddSystemsManager( $"/attain/my/{builder.Environment.EnvironmentName}/" );
    }
}
