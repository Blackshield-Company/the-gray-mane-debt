using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using static GrayManeDebt.Generator.Vanilla;

namespace GrayManeDebt.Generator;

/// <summary>
/// Generates GrayManeDebt.esp (ESL-flagged), its SEQ file and the silent voice files.
/// Usage: generator --out &lt;dir&gt; --ids &lt;formids.json&gt; --papyrus &lt;src/papyrus&gt; [--report &lt;file&gt;]
/// Everything that belongs in Data\ is written under --out.
/// </summary>
internal static class Program
{
    public const string PluginName = "GrayManeDebt.esp";
    private static readonly SkyrimRelease Release = SkyrimRelease.SkyrimSE;

    private static int Main(string[] args)
    {
        string Arg(string name, string fallback)
        {
            var i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
        var outDir = Arg("--out", "build/plugin");
        var idsPath = Arg("--ids", "src/plugin/formids.json");
        var papyrusDir = Arg("--papyrus", "src/papyrus");
        var reportPath = Arg("--report", Path.Combine(Path.GetDirectoryName(Path.GetFullPath(outDir))!, "plugin-report.json"));
        var voiceListPath = Arg("--voice-list", Path.Combine(Path.GetDirectoryName(Path.GetFullPath(outDir))!, "voice-files.tsv"));
        if (Directory.Exists(outDir)) Directory.Delete(outDir, true);
        Directory.CreateDirectory(outDir);

        var modKey = ModKey.FromFileName(PluginName);
        var ids = new FormIdRegistry(idsPath, modKey);
        var mod = new SkyrimMod(modKey, Release);
        var builder = new Builder(mod, ids);
        builder.Build();

        mod.ModHeader.Flags |= SkyrimModHeader.HeaderFlag.Small; // ESL-flagged ESP
        mod.ModHeader.Stats.Version = 1.7f;
        mod.ModHeader.Author = "synthalorian";
        mod.ModHeader.Description = "The Gray-Mane Debt: a short questline for Eorlund Gray-Mane with unvoiced dialogue and three endings.";

        var outPath = Path.Combine(outDir, PluginName);
        mod.BeginWrite.ToPath(outPath).WithNoLoadOrder().Write();
        ids.Save();
        foreach (var r in ids.Retired) Console.WriteLine($"[generate] note: retired EditorID kept reserved: {r}");
        Console.WriteLine($"[generate] wrote {outPath}");

        // Master indices are only final in the written file, so the SEQ is built from it.
        int seq;
        using (var written = SkyrimMod.CreateFromBinaryOverlay(outPath, Release)) seq = SeqFile.Write(outDir, written);
        Console.WriteLine($"[generate] SEQ: {seq} start-game-enabled quest(s)");

        var lines = SilentVoice.Plan(mod, t => new[] { builder.Speakers[t.FormKey] });
        SilentVoice.Write(outDir, lines);
        File.WriteAllLines(voiceListPath, new[] { "file\tvoice_type\tspeaker\tseconds\ttext" }
            .Concat(lines.Select(l => $"{l.RelativePath}\t{l.VoiceType}\t{l.Speaker}\t{l.Seconds:0.0}\t{l.Text}")));
        Console.WriteLine($"[generate] {lines.Count} silent voice files ({lines.Sum(l => l.Seconds):0} s), list in {voiceListPath}");

        var errors = PluginValidator.Validate(outPath, new PapyrusIndex(papyrusDir), new HashSet<ModKey> { Skyrim }, reportPath, builder.ExpectedUnfilled);
        return errors == 0 ? 0 : 1;
    }
}
