using System.Text.Json;

const string defaultSnapshot = "snapshots/ai_generated_change";
var snapshotArg = args.Length > 0 ? args[0] : defaultSnapshot;
var baseDir = AppContext.BaseDirectory;
var workspaceDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
var snapshotPath = Path.GetFullPath(Path.Combine(workspaceDir, snapshotArg));
var rulesPath = Path.Combine(baseDir, "rules.json");

if (!Directory.Exists(snapshotPath))
{
    Console.WriteLine($"Snapshot not found: {snapshotPath}");
    return;
}

var config = JsonSerializer.Deserialize<RuleConfig>(
    File.ReadAllText(rulesPath),
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
);

if (config is null || config.Rules.Count == 0)
{
    Console.WriteLine("No rules loaded.");
    return;
}


var findings = new List<Finding>();
var files = Directory.GetFiles(snapshotPath, "*.*", SearchOption.AllDirectories)
    .Where(x => x.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ||
                x.EndsWith(".ts", StringComparison.OrdinalIgnoreCase) ||
                x.EndsWith(".tsx", StringComparison.OrdinalIgnoreCase))
    .ToList();

foreach (var file in files)
{
    var content = File.ReadAllText(file);
    var relative = Path.GetRelativePath(snapshotPath, file).Replace("\\", "/");

    foreach (var rule in config.Rules)
    {
        if (!relative.Contains(rule.TargetContains, StringComparison.OrdinalIgnoreCase))
        {
            continue;
        }

        if (!relative.Contains(rule.FileNameContains, StringComparison.OrdinalIgnoreCase))
        {
            continue;
        }

        if (!content.Contains(rule.ContainsText, StringComparison.Ordinal))
        {
            continue;
        }

        findings.Add(new Finding(
            rule.RuleId,
            rule.Severity,
            relative,
            rule.Message,
            rule.Recommendation
        ));
    }
}

Console.WriteLine($"Snapshot: {snapshotArg}");
Console.WriteLine($"Files scanned: {files.Count}");
Console.WriteLine(new string('-', 80));

if (findings.Count == 0)
{
    Console.WriteLine("No findings.");
}
else
{
    foreach (var f in findings)
    {
        Console.WriteLine($"[{f.Severity}] {f.RuleId} :: {f.File}");
        Console.WriteLine($"  Message: {f.Message}");
        Console.WriteLine($"  Recommendation: {f.Recommendation}");
    }
}

Console.WriteLine(new string('-', 80));
var errors = findings.Count(x => x.Severity.Equals("ERROR", StringComparison.OrdinalIgnoreCase));
var warnings = findings.Count(x => x.Severity.Equals("WARNING", StringComparison.OrdinalIgnoreCase));
var decision = errors > 0 ? "FAIL" : warnings > 0 ? "WARN" : "PASS";

Console.WriteLine($"Totals -> ERROR: {errors}, WARNING: {warnings}");
Console.WriteLine($"Gate decision: {decision}");

public sealed record Finding(string RuleId, string Severity, string File, string Message, string Recommendation);

public sealed class RuleConfig
{
    public List<RuleDefinition> Rules { get; init; } = new();
}

public sealed class RuleDefinition
{
    public string RuleId { get; init; } = "";
    public string Severity { get; init; } = "";
    public string TargetContains { get; init; } = "";
    public string FileNameContains { get; init; } = "";
    public string ContainsText { get; init; } = "";
    public string Message { get; init; } = "";
    public string Recommendation { get; init; } = "";
}
