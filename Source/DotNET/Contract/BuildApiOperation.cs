// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// A command or query a backend build exposes.
/// </summary>
/// <param name="Kind">Whether it is a command or a query.</param>
/// <param name="Name">The name of the operation.</param>
/// <param name="Method">The HTTP method, for a command or query exposed over HTTP.</param>
/// <param name="Route">The route, for a command or query exposed over HTTP.</param>
public record BuildApiOperation(BuildApiOperationKind Kind, string Name, string? Method = null, string? Route = null);
