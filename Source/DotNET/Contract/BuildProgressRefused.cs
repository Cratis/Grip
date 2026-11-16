// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Why a progress report was refused: the reasons it was not read.
/// </summary>
/// <param name="Reasons">Why it was refused, one reason per problem.</param>
public record BuildProgressRefused(IReadOnlyList<string> Reasons);
