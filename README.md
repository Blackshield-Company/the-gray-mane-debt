# The Gray-Mane Debt

A short Skyrim Special Edition questline for **Eorlund Gray-Mane**, the smith at the Skyforge. A thief has stolen the notched ivory pattern every Skyforge blade is folded by, and the trail leads to a debt Eorlund would rather not talk about. Find the thief, face the moneylender, and decide whether the Gray-Mane debt is settled with coin, with steel, or by selling Eorlund out.

**Status: v0.1.0, early beta.** It passes every automated check in the build (Papyrus compile, plugin and dialogue validation, byte-exact round trip, voice file and SEQ checks, FOMOD schema) but has not been playtested in-game yet. Expect rough edges and please report what you find. See [docs/TESTING.md](docs/TESTING.md).

- Game: Skyrim Special Edition / Anniversary Edition 1.6.x (also fine on 1.5.97). Works on Windows and on Linux through Proton.
- Requirements: none beyond the base game. No SKSE, no SkyUI, no DLC, no Creation Club content.
- Plugin: `GrayManeDebt.esp`, ESL-flagged, Skyrim.esm is the only master. **No vanilla records edited**: not Eorlund, not his AI, not his dialogue.
- Compatible with the rest of the series ([Blackshield Company](https://github.com/synthalorian/blackshield-company), [Bounties of Oblivion](https://github.com/synthalorian/bounties-of-oblivion), [Contract Hunter](https://github.com/synthalorian/contract-hunter), [Steel Oath](https://github.com/synthalorian/steel-oath), [Ember Smithing](https://github.com/synthalorian/ember-smithing)): own prefix (`GMD_`), records, globals and files. Checked by loading all plugins together (see [tools/modkit](tools/modkit/README.md)).

## The quest

1. **"You look troubled, Eorlund."** Talk to Eorlund at the Skyforge (level 5+). He tells you someone broke in and took his grandfather's tempering pattern, and hands you a note the thief dropped.
2. **The note** names a hideout: a dungeon in Whiterun, Falkreath, Hjaalmarch, the Pale or Eastmarch, picked when the quest starts. The thief, Brund Coal-Hand, waits at its entrance.
3. **The pattern** is on Brund, with a letter from the man who hired him: **Hrodulf Ash-Purse**, a moneylender now waiting at the Whiterun Stables with two enforcers.
4. **Eorlund's confession**: bring the pattern home and he tells you about the debt.
5. **The choice**, made in dialogue with Hrodulf:

| Ending | How | Reward |
|---|---|---|
| Pay the debt | Settle it with Hrodulf (1,000 gold) after Eorlund has explained, then tell Eorlund | **Gray-Mane Heirloom** (steel sword, 13 damage, frost enchantment) and two Smithing lessons from Eorlund |
| Fight | Refuse Hrodulf in dialogue and win the fight (or kill him some other way; attacking him unprovoked is a crime like any assault), then tell Eorlund | **Gray-Mane Heirloom**, plus what Hrodulf carried |
| Betray Eorlund | While you still have the pattern, sell it to Hrodulf | 2,000 gold. Eorlund has only one thing to say to you afterwards |

If Eorlund dies before it is over, the quest fails with a journal entry and everything it spawned is cleaned up.

## Unvoiced dialogue

All 27 new lines (18 dialogue entries) are unvoiced, shown as subtitles. The engine plays a line for as long as its voice file lasts, so the mod ships a **silent voice file for every line**, sized to its length (1 s plus 1 s per 2.6 words, 2-12 s). Turn subtitles on (*Settings → Display → Dialogue Subtitles*). See [docs/DESIGN.md](docs/DESIGN.md#silent-voice-files) for how they are made. If you use Fuz Ro D'oh, nothing changes: it only fills in for lines that have no file.

## Install

Grab `GrayManeDebt-<version>.zip` from [Releases](https://github.com/synthalorian/the-gray-mane-debt/releases) (FOMOD installer).

- **Mod Organizer 2**: *Install a new mod from an archive*, pick the zip, enable `GrayManeDebt.esp`.
- **Vortex**: drag the zip into the Mods page, install and enable it.
- **Manual (Linux/Proton)**: copy everything inside `00 Core/` (the plugin, `Scripts/`, `SEQ/` and `Sound/`) into `.../steamapps/common/Skyrim Special Edition/Data/`, then add `*GrayManeDebt.esp` to `.../steamapps/compatdata/489830/pfx/drive_c/users/steamuser/AppData/Local/Skyrim Special Edition/Plugins.txt`.

Safe to add to an existing save. Finish or fail the quest before uninstalling.

## Console tunables

```
set GMD_MinLevel to 5          (level needed before Eorlund brings it up)
set GMD_DebtAmount to 1000     (what the Gray-Manes owe Hrodulf)
set GMD_BetrayGold to 2000     (what Hrodulf pays for the pattern)
set GMD_SmithingLessons to 2   (Smithing levels Eorlund teaches in the "pay" ending)
```

## Compatibility notes

- **Companions**: the opening topic is hidden while *Glory of the Dead* runs. The questline adds its own topics and never touches the Companions' or Eorlund's vanilla dialogue, so Companions quests that use Eorlund work as before.
- **Missing in Action** (the Gray-Mane / Battle-Born quest): no shared records, aliases or conditions. Both can run at the same time.
- **Civil War**: the opening topic is hidden while a city siege is running. Eorlund stays in Whiterun under either side, so nothing else changes.
- **Mods that edit Eorlund** (appearance, AI, voice type) are fine, with one exception: a mod that changes his voice type moves his lines away from the silent voice files, so they flash past again. Fuz Ro D'oh covers that case.
- **The Whiterun Stables**: Hrodulf and his enforcers wait at the stables' map marker. Mods that rebuild the stables area may leave them standing somewhere odd but still reachable.

## Build from source

`./build.sh` on Linux builds the release zip from this repo: Papyrus is compiled with the open-source [papyrus-compiler](https://github.com/russo-2025/papyrus-compiler), the plugin is generated from C# with [Mutagen](https://github.com/Mutagen-Modding/Mutagen), the SEQ file and silent voice files are written next to it, and everything is validated (ESL range, masters, links, script properties and base types, alias scripts, dialogue fragments and structure, a voice file for every response, the SEQ contents, byte-exact round trip). The FOMOD is schema-checked and zipped reproducibly. Needs .NET SDK 9+, python3, curl and optionally xmllint. See [tools/modkit/README.md](tools/modkit/README.md).

`src/plugin/formids.json` pins every record's FormID. Never edit or reuse entries: the voice file names contain the dialogue FormIDs.

## Layout

```
src/plugin/GrayManeDebt.Generator/   C# generator (records, quests, aliases, dialogue)
src/plugin/formids.json              pinned FormIDs
src/papyrus/                         quest, alias and dialogue fragment scripts
fomod/                               FOMOD installer config
tools/modkit/                        shared build tooling (Mutagen helpers, validator, Papyrus, FOMOD, compat check)
docs/                                design notes, testing checklist, Nexus page
```

## Credits

- Built with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) and [russo-2025/papyrus-compiler](https://github.com/russo-2025/papyrus-compiler).
- Eorlund Gray-Mane, the Skyforge and Whiterun belong to Bethesda Softworks. This mod references them from Skyrim.esm and ships no Bethesda assets.

## License

MIT (code and text in this repository). See [LICENSE](LICENSE).
