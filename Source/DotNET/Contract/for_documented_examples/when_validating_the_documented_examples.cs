// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System.Text.Json;

namespace Cratis.Grip.Contract.for_documented_examples;

// The examples builders copy from (the JSON code blocks of Documentation/contract/orders-and-outcomes.md) must be
// documents the contract accepts.
public class when_validating_the_documented_examples : Specification
{
    readonly Dictionary<string, IReadOnlyList<string>> _problems = [];
    readonly Dictionary<string, bool> _outcomesRead = [];
    readonly Dictionary<string, bool> _progressRead = [];
    readonly List<string> _parts = [];

    void Because()
    {
        var examples = 0;
        foreach (var text in JsonExamples())
        {
            examples++;
            var name = $"example {examples}";
            using var document = JsonDocument.Parse(text);
            var root = document.RootElement;
            if (root.TryGetProperty("part", out _))
            {
                var version = root.GetProperty("contractVersion").GetString()!;
                _parts.Add(root.GetProperty("part").GetString()!);
                _problems[name] = BuildContractValidation.Problems(BuildContractSchemas.Order, version, root);
            }
            else if (root.TryGetProperty("kind", out _))
            {
                _outcomesRead[name] = BuildOutcomeReader.Read(text).TryGetResult(out _);
            }
            else
            {
                _progressRead[name] = BuildProgressReader.Read(text).TryGetResult(out _);
            }
        }
    }

    [Fact] void should_have_an_order_example_of_every_part() => _parts.ShouldContainOnly("backend", "ui", "removal");
    [Fact] void should_have_an_example_of_every_outcome_and_one_with_usage() => _outcomesRead.Count.ShouldEqual(5);
    [Fact] void should_have_a_progress_example() => _progressRead.Count.ShouldEqual(1);
    [Fact] void should_read_every_progress_report() => _progressRead.Where(progress => !progress.Value).Select(progress => progress.Key).ShouldBeEmpty();
    [Fact] void should_have_only_valid_orders() => _problems.Values.SelectMany(problems => problems).ShouldBeEmpty();
    [Fact] void should_read_every_outcome() => _outcomesRead.Where(outcome => !outcome.Value).Select(outcome => outcome.Key).ShouldBeEmpty();

    static IEnumerable<string> JsonExamples()
    {
        var page = Path.Combine(DocumentationFolder(), "contract", "orders-and-outcomes.md");
        var inside = false;
        var block = new List<string>();
        foreach (var line in File.ReadAllLines(page))
        {
            if (!inside && line.Trim() == "```json")
            {
                inside = true;
                block.Clear();
            }
            else if (inside && line.Trim() == "```")
            {
                inside = false;
                yield return string.Join('\n', block);
            }
            else if (inside)
            {
                block.Add(line);
            }
        }
    }

    static string DocumentationFolder()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            var documentation = Path.Combine(directory.FullName, "Documentation", "contract");
            if (Directory.Exists(documentation)) return Path.GetDirectoryName(documentation)!;
        }

        throw new DirectoryNotFoundException("Documentation/contract was not found above the test output.");
    }
}
#endif
