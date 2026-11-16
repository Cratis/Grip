// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Kits;

/// <summary>
/// The exception thrown when the text of a kit manifest is not readable as YAML or JSON.
/// </summary>
/// <param name="reason">Why the text could not be read.</param>
public class BuildKitManifestUnreadable(string reason)
    : Exception($"The build kit manifest cannot be read: {reason}");
