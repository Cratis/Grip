// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The tokens an agent builder used.
/// </summary>
/// <param name="InputTokens">The tokens read.</param>
/// <param name="OutputTokens">The tokens written.</param>
public record BuildUsage(long InputTokens, long OutputTokens);
