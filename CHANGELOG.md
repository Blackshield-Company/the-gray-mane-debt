# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

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
