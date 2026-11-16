// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What happened to a specification when the builder ran it.
/// </summary>
/// <param name="Id">The identifier of the specification.</param>
/// <param name="Result">The verdict.</param>
/// <param name="Message">A message about the verdict, or null.</param>
public record BuildSpecificationResult(Guid Id, BuildSpecificationVerdict Result, string? Message);
