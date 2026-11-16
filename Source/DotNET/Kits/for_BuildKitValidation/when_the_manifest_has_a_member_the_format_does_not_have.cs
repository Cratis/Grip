// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_manifest_has_a_member_the_format_does_not_have : given.a_kit
{
    void Because()
    {
        Manifest(ValidManifest + "\ntoken: secret-value\n");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
}
#endif
