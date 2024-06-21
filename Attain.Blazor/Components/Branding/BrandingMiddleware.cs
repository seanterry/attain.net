// SPDX-License-Identifier: Proprietary
// Copyright (c) TTCO Holding Company, Inc. and Contributors
// All Rights Reserved.

namespace Attain.Components.Branding;

public class BrandingMiddleware( BrandContext brandContext, IHostEnvironment environment, IConfiguration configuration ) : IMiddleware, IScoped
{
    Brand GetBrand( HttpContext context )
    {
        if ( environment.IsDevelopment() ) return configuration.GetValue<Brand>( "Brand" );

        return context.Request.Host.Host switch
        {
            var host when host.EndsWith( ".empoweriep.com", StringComparison.OrdinalIgnoreCase ) => Brand.Empower,
            _ => Brand.Attain
        };
    }

    public async Task InvokeAsync( HttpContext context, RequestDelegate next )
    {
        var brand = GetBrand( context );
        brandContext.Change( brand );
        await next( context );
    }
}
