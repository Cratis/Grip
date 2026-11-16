// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The application the order belongs to.
/// </summary>
/// <param name="Id">The identifier of the application.</param>
/// <param name="Name">The name of the application.</param>
public record BuildScopeApplication(Guid Id, string Name);
