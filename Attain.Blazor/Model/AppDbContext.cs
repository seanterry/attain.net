// SPDX-License-Identifier: Proprietary
// Copyright (c) TTCO Holding Company, Inc. and Contributors
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;

namespace Attain.Model;

public sealed class AppDbContext( DbContextOptions<AppDbContext> options ) : DbContext( options )
{

}
