# Nexus Mods page: The Gray-Mane Debt

Early beta: v0.1.1 passed every automated build check but hasn't been playtested in-game. The page says so up front. Exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** The Gray-Mane Debt
- **Version:** 0.1.1 (early beta)
- **Category:** Quests and Adventures
- **Suggested tags:** Quests, Lore-Friendly, Dialogue, Whiterun, Choices and Consequences, ESL, No SKSE, Scripted
- **Requirements:** none.
- **Permissions (suggested):** source on GitHub under MIT, so allow modification and conversions with credit. Don't allow re-uploads of the archive itself.
- **File to upload:** `GrayManeDebt-0.1.1.zip` (FOMOD), main file, named "The Gray-Mane Debt".

## Description (paste into the BBCode editor)

```bbcode
[center][size=6][b]THE GRAY-MANE DEBT[/b][/size]
[size=3][i]Every Skyforge blade is folded by an old notched pattern. Someone stole it. Eorlund knows why.[/i][/size]

[size=3][color=#e0a030][b]EARLY BETA (v0.1.1)[/b]: this version passes every automated build check but hasn't been playtested in-game yet. Please report bugs in the Bugs tab or on GitHub, ideally with your Papyrus log.[/color][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
A short questline for [b]Eorlund Gray-Mane[/b], the smith at the Skyforge. Someone broke into the forge and took his grandfather's tempering pattern, a strip of notched ivory that every fold of Skyforge steel is counted by. Track the thief, recover the pattern, and learn about the debt the Gray-Manes owe a dangerous moneylender. Then decide how it ends.

[size=4][b]The quest[/b][/size]
[list]
[*]Talk to Eorlund at the Skyforge (level 5+): [i]"You look troubled, Eorlund."[/i]
[*]Read the note the thief dropped. It leads to a dungeon in Whiterun or a neighboring hold, different each time.
[*]Recover the pattern, and the letter that names the man who hired the thief: [b]Hrodulf Ash-Purse[/b].
[*]Bring the pattern home and hear Eorlund's confession.
[*]Face Hrodulf and his enforcers at the Whiterun Stables.
[/list]

[size=4][b]Three endings[/b][/size]
[list]
[*][b]Pay the debt[/b] (1,000 gold): Eorlund gives you his father's sword, the [b]Gray-Mane Heirloom[/b], and teaches you two levels of Smithing.
[*][b]Fight[/b]: refuse Hrodulf and settle it with steel. The Heirloom is yours, and whatever Hrodulf carried.
[*][b]Betray Eorlund[/b]: sell Hrodulf the pattern for 2,000 gold. Eorlund will have only one thing to say to you afterwards.
[/list]

[size=4][b]Unvoiced dialogue that you can actually read[/b][/size]
The new lines are unvoiced and shown as subtitles. Every line ships with a silent voice file sized to its length, so subtitles stay on screen instead of flashing past. [b]No Fuz Ro D'oh needed[/b] (and it doesn't conflict if you have it). Turn on dialogue subtitles in the game settings.

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]GrayManeDebt.esp[/b]. It's ESL-flagged, so it doesn't take a full load-order slot. Safe to add mid-playthrough.

[size=4][b]Compatibility[/b][/size]
[list]
[*][b]No edits to vanilla records[/b]: not Eorlund's NPC record, not his AI, not his dialogue, no cells or navmesh. New topics are conditioned on Eorlund instead. Skyrim.esm is the only master.
[*][b]Companions[/b]: the opening is hidden during Glory of the Dead; Companions quests that use Eorlund are untouched.
[*][b]Missing in Action[/b] (Gray-Mane / Battle-Born): no overlap, both can run together.
[*][b]Civil War[/b]: the opening is hidden during a city siege. Eorlund stays at the Skyforge under either side.
[*][b]If Eorlund dies[/b], the quest won't start, or fails cleanly with a journal entry if it was running.
[*]Mods that change Eorlund's [i]voice type[/i] move his lines away from the silent voice files (they'd flash past again). Appearance and AI mods are fine.
[*]Compatible with [b]Blackshield Company[/b], [b]Bounties of Oblivion[/b], [b]Contract Hunter[/b], [b]Steel Oath[/b] and [b]Ember Smithing[/b] (same author), checked by loading all plugins together.
[/list]

[size=4][b]Uninstalling[/b][/size]
As with any scripted mod, don't remove it mid-playthrough. Finish or fail the quest first.

[size=4][b]Known issues / early beta limits[/b][/size]
[list]
[*]Not playtested in-game yet. Found something? Please post it in the Bugs tab.
[*]No scenes or custom AI: Eorlund tells his story in dialogue at the forge, and Hrodulf waits at the stables rather than arriving with fanfare.
[*]No lip movement on the new lines (silent WAV files; FUZ/LIP generation needs Windows-only tools).
[*]Betraying Eorlund changes his lines in this quest only; he still trades and trains as usual.
[*]Settings are console globals: [code]set GMD_MinLevel to 5 / set GMD_DebtAmount to 1000 / set GMD_BetrayGold to 2000 / set GMD_SmithingLessons to 2[/code]
[/list]

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/synthalorian/the-gray-mane-debt]github.com/synthalorian/the-gray-mane-debt[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors, which generates the plugin from code.
[*][url=https://github.com/russo-2025/papyrus-compiler]papyrus-compiler[/url] by russo-2025, the open-source Papyrus compiler.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]
```

## Screenshots to capture in-game

Take these at 16:9, with the HUD hidden for scenery shots (`tm` in the console toggles menus).

1. **Hero shot.** Eorlund at the Skyforge with the eagle behind him. This goes on the mod tile.
2. **The opening.** The topic list with "You look troubled, Eorlund." and his first subtitle.
3. **The note.** The Soot-Stained Note open.
4. **The hideout.** Brund Coal-Hand deep in a dungeon (at its entrance if the dungeon has no boss spot).
5. **The confession.** Eorlund's lines about the debt.
6. **Hrodulf.** Hrodulf Ash-Purse and his enforcers at the Whiterun Stables, with the choice list.
7. **The reward.** The Gray-Mane Heirloom in the inventory.
