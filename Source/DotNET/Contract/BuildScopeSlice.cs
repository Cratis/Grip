// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// A slice to build, with its type and its status in the model.
/// </summary>
/// <param name="Id">The identifier of the slice.</param>
/// <param name="Name">The name of the slice.</param>
/// <param name="Type">The type of the slice.</param>
/// <param name="Status">The status of the slice in the model.</param>
public record BuildScopeSlice(Guid Id, string Name, string Type, string Status);
