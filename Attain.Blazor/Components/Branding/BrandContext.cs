// SPDX-License-Identifier: Proprietary
// Copyright (c) TTCO Holding Company, Inc. and Contributors
// All Rights Reserved.

namespace Attain.Components.Branding;

/// <summary>
/// Context for accessing or changing the current brand.
/// </summary>
public class BrandContext : IScoped
{
    public Brand Current { get; private set; } = Brand.Attain;

    public event EventHandler<Brand>? BrandChanged;

    public void Change( Brand brand )
    {
        if ( Current == brand ) return;

        Current = brand;
        BrandChanged?.Invoke( this, brand );
    }

    /// <summary>
    /// Application name based on the current brand.
    /// </summary>
    public string Name => Current switch
    {
        Brand.Empower => "empoWEr",
        _ => "SEAS 3.0"
    };

    /// <summary>
    /// Application theme for the current brand.
    /// </summary>
    public string Theme => Current switch
    {
        Brand.Empower => "empower",
        _ => "attain"
    };

    /// <summary>
    /// Name of the business entity based on the current brand.
    /// </summary>
    public string Company => Current switch
    {
        Brand.Empower => "TCASE",
        _ => "TTCO Holding Company, Inc."
    };
}
