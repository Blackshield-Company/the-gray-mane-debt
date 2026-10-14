# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.3] - 2026-10-14

Still early beta, not yet playtested in game. Save-compatible (no records added or removed, no form IDs changed).

### Fixed
- Hrodulf's note to Brund showed a raw location tag (`<Alias=ThiefLocation>`) instead of the hideout's name. The note now uses the quest's stored text.

### Changed
- The build check now fails on any `<Alias=...>` tag in dialogue (responses, prompts, topic text) or quest text (name, journal, objectives) that is not a filled, non-optional alias of the quest that owns the text, and on alias-created notes whose tags would not resolve.

## [0.1.2] - 2026-10-10

Still early beta, not yet playtested in game.

### Fixed
- The quest could stall at "Recover the Skyforge pattern" when Brund only had Hrodulf's letter. Only picking up the pattern advanced the quest, so the marker stayed on his body. Now, taking the letter while the pattern is not on Brund gives you the pattern and moves the quest on.
- Fix for saves already stuck (including 0.1.0 and 0.1.1 saves): with the letter in your inventory, ask Eorlund "Brund didn't have the pattern. Just this letter." He hands you the pattern and the quest continues.

## [0.1.1] - 2026-10-10

Still early beta, not yet playtested in game.

### Fixed
- The Skyforge pattern (and Hrodulf's letter) could be missing from Brund Coal-Hand. The quest now gives them to him by script when it starts, and checks again when he loads and when he dies, re-adding them if missing. If you already carry the pattern, the quest advances on the next check. Saves already looking for Brund get the fix the next time he loads.

### Changed
- Brund now waits at the dungeon's boss spot instead of the outdoor map marker. Dungeons without one keep the map marker.

## [0.1.0] - 2026-10-06

Early beta. First public release. It passes every automated check in the build (Papyrus compile, plugin and dialogue validation, round trip, voice file and SEQ checks, FOMOD schema) but has not been playtested in-game yet. Please report problems on the Nexus page or on GitHub.

### Added
- A short questline for Eorlund Gray-Mane: "You look troubled, Eorlund." opens it at the Skyforge once you are level 5 (`GMD_MinLevel`).
- Six journal stages across two quests: read the thief's note, track the thief to a dungeon in Whiterun or a neighboring hold (picked at random), recover the Skyforge tempering pattern, learn the truth about the Gray-Mane debt, confront the moneylender Hrodulf Ash-Purse at the Whiterun Stables, and report back.
- Three endings with different rewards:
  - Pay the debt (1,000 gold, `GMD_DebtAmount`): Eorlund's heirloom sword and two Smithing lessons.
  - Fight Hrodulf and his enforcers: the heirloom sword and whatever Hrodulf carried.
  - Sell the pattern to Hrodulf: 2,000 gold (`GMD_BetrayGold`) and a cold shoulder from Eorlund from then on.
- Unvoiced dialogue (27 lines in 18 entries for Eorlund and Hrodulf) with silent voice files sized to each line, so subtitles stay up long enough to read.
- New items: Soot-Stained Note, Hrodulf's Letter, Skyforge Tempering Pattern, and the Gray-Mane Heirloom (steel sword, frost enchantment).
- Clean failure: the quest cannot start if Eorlund is dead, and fails (journal entry, no stuck objectives) if he dies while it runs. The opening topic stays hidden during Glory of the Dead and during a city siege.
- `SEQ/GrayManeDebt.seq` for the Start Game Enabled dialogue quest.
- FOMOD installer with optional Papyrus sources.
