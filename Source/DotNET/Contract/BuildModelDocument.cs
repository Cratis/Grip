// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// One document of the model, with its path and its content.
/// </summary>
/// <param name="Path">The path of the document in the model.</param>
/// <param name="Content">The content of the document.</param>
public record BuildModelDocument(string Path, string Content);
