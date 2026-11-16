// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Path names the module and any parent features, outermost first.
/// </summary>
/// <param name="Id">The identifier of the feature.</param>
/// <param name="Name">The name of the feature.</param>
/// <param name="Path">The modules and parent features down to the feature, outermost first.</param>
public record BuildScopeFeature(Guid Id, string Name, IReadOnlyList<string> Path);
