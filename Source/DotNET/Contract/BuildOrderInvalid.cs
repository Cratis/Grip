// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Why an order could not be written: the reasons it does not match its schema.
/// </summary>
/// <param name="Reasons">Why it was refused, one reason per problem.</param>
public record BuildOrderInvalid(IReadOnlyList<string> Reasons);
