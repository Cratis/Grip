// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Where in the application the order builds: the application, the feature and the slices to build.
/// </summary>
/// <param name="Application">The application.</param>
/// <param name="Feature">The feature.</param>
/// <param name="Slices">The slices to build.</param>
public record BuildScope(BuildScopeApplication Application, BuildScopeFeature Feature, IReadOnlyList<BuildScopeSlice> Slices);
