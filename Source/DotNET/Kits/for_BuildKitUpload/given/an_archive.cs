// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System.IO.Compression;
using System.Text;

namespace Cratis.Grip.Kits.for_BuildKitUpload.given;

public class an_archive : Specification
{
    protected BuildKitUploadContent _result = null!;

    protected static byte[] Zip(params string[] paths)
    {
        using var buffer = new MemoryStream();
        using (var zip = new ZipArchive(buffer, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var path in paths)
            {
                using var stream = zip.CreateEntry(path).Open();
                stream.Write(Encoding.UTF8.GetBytes(path));
            }
        }

        return buffer.ToArray();
    }
}
#endif
