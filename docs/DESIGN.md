# The Gray-Mane Debt: design notes

## Goals and constraints

- A questline started and driven by Eorlund Gray-Mane with **unvoiced dialogue**, without touching his NPC record, his AI packages or any vanilla dialogue.
- Base game only (Skyrim.esm), ESL-flagged, no SKSE, no edits to vanilla records, no placed world objects.
- Must not break the Companions questline or *Missing in Action*, and must cope with Eorlund dying and with the Civil War.

## Records

| Type | EditorID | Purpose |
|---|---|---|
| QUST | `GMD_Intro` | Start Game Enabled. Owns the opening topic and Eorlund's after-quest lines. No aliases. Listed in `SEQ/GrayManeDebt.seq`. |
| QUST | `GMD_Main` | The questline (stages, objectives, aliases, Eorlund and Hrodulf topics). Started by script when you accept. |
| DLBR/DIAL/INFO | `GMD_*Branch`, `GMD_I_*`, `GMD_E_*`, `GMD_C_*` | 6 top-level branches, 12 topics, 18 infos, 27 responses. |
| NPC_ | `GMD_Thief` | Brund Coal-Hand. Templated on the vanilla Nord bandit melee list (looks, gear, factions, AI), so he is hostile like any bandit. |
| NPC_ | `GMD_Hrodulf` | Hrodulf Ash-Purse. Only *Traits* (face, race, voice) come from the vanilla guard look NPC `EncGuardSonsM03MaleNordCommander`, which has its own FaceGen and the MaleNordCommander voice type. Stats, gear (fine clothes, steel sword, 350 gold), faction and AI are his own: unaggressive until the fight. |
| NPC_ | `GMD_Enforcer` | Hrodulf's two enforcers. Bandit looks, gear and combat; Hrodulf's faction and unaggressive AI. |
| FACT | `GMD_CreditorFaction` | Hrodulf's crew. Made an enemy of the player's faction only when you choose to fight. |
| FLST | `GMD_ThiefHolds` | Whiterun, Falkreath, Hjaalmarch, the Pale, Eastmarch: where the thief may hide. |
| BOOK | `GMD_ThiefNote`, `GMD_HrodulfLetter` | The note uses `<Alias=ThiefLocation>`; both are quest-created so the text tags resolve. |
| MISC | `GMD_SkyforgePattern` | The pattern (vanilla horker tusk mesh). Quest object while the quest runs. |
| WEAP | `GMD_HeirloomBlade` | Gray-Mane Heirloom: 13 damage steel sword (vanilla Skyforge steel sword mesh and sounds), vanilla `EnchWeaponFrostDamage03`, 2000 charge. |
| GLOB | `GMD_State`, `GMD_MinLevel`, `GMD_DebtAmount`, `GMD_BetrayGold`, `GMD_SmithingLessons` | State (0 new, 1 running, 2 helped, 3 betrayed, 4 failed) and tunables. |
| MESG | `GMD_MsgTrailCold` | Shown if `GMD_Main` cannot start. |

53 records, FormIDs 800-834. No overrides.

## Flow

| Stage | Trigger | What happens |
|---|---|---|
| 10 | You accept Eorlund's request (fragment `GMD_TIF_Accept` → `GMD_IntroScript.Accept`) | `GMD_Main` starts; its aliases fill: Eorlund (unique actor), a dungeon for the thief, the thief at its map marker, the pattern and letter in his inventory, the note in yours, Hrodulf and two enforcers created *disabled* at the Whiterun Stables map marker. |
| 20 | You read the note (Ticket alias `OnRead`) | Objective: recover the pattern at the hideout. |
| 30 | The pattern enters your inventory (Pattern alias `OnContainerChanged`) | Hrodulf and the enforcers are enabled. Objectives: return the pattern to Eorlund, or (optional) sell it to Hrodulf. |
| 40 | "I recovered your pattern." (fragment `GMD_TIF_ReturnPattern`) | The pattern goes back to Eorlund; he tells you about the debt. Objective: settle it with Hrodulf. |
| 50 | "I'm here to settle the Gray-Mane debt in full." (needs `GMD_DebtAmount` gold) | Gold removed; Hrodulf and his men leave. Objective: tell Eorlund. |
| 60 | Hrodulf dies (Creditor alias `OnDeath`), at stage 30 or 40 | Objective: tell Eorlund. "The Gray-Manes owe you nothing. Draw your blade." makes the crew hostile (faction enemy + StartCombat). |
| 100 | "The debt is paid." | Heirloom sword, `GMD_SmithingLessons` Smithing levels (`Game.IncrementSkill`). Completed. |
| 110 | "Hrodulf is dead." | Heirloom sword (and the pattern goes home if you still had it). Completed. |
| 120 | "I have the Skyforge pattern. What's it worth to you?" (stage 30 only) | Pattern removed, `GMD_BetrayGold` gold, Hrodulf leaves. Completed (betrayal). Eorlund's only line afterwards is a cold one. |
| 200 | Eorlund dies (Eorlund alias `OnDeath`) before 100 | All objectives failed, journal entry. Failed. |

After 100/110/120/200 the quest stops a few seconds later, which deletes everything it created. `GMD_State` remembers the outcome for `GMD_Intro`'s lines.

## Dialogue

- Every info is conditioned on its speaker: `GetIsID EorlundGrayMane` for Eorlund (his base record is only *read*), `GetIsAliasRef Creditor` for Hrodulf. Choices are linked topics (TCLT) in the same branch.
- The opening info also requires `GMD_State == 0`, player level ≥ `GMD_MinLevel`, `GMD_Main` not running, `C06` (*Glory of the Dead*, where Eorlund works the funeral pyre) not running and `CWSiegeObj` (a city siege) not running.
- After the quest, the same topic shows "How's the forge, Eorlund?" (helped) or "Eorlund..." (betrayed) through prompt overrides.
- Fragments are one-line TopicInfo scripts (`GMD_TIF_*`) that call the quest script, bound to the info's end so they run after the line is spoken. The validator checks each one: the fragment file is attached, the function exists and takes a single `ObjectReference`.
- Quest and topic EditorIDs are kept at ≤ 10 and ≤ 15 characters so voice file names are never truncated.

## Silent voice files

The engine shows an unvoiced line only for as long as its voice file lasts; with no file it flashes past. The standard fix is to ship a silent file per response at the path the engine looks for:

```
Sound/Voice/GrayManeDebt.esp/<VoiceType>/<QuestEditorID>_<TopicEditorID>_<INFO FormID>_<response>.wav
```

- **Voice type**: Eorlund's NPC record (013B9D) uses voice type 0E5003, **MaleNordCommander** (checked against his vanilla record; not MaleNord or MaleEvenToned). Hrodulf's Traits template has the same voice type, so every line lives in `MaleNordCommander/`.
- **FormID**: the INFO's plugin-local id with a `00` load-order byte, e.g. `00000815`. For an ESL-flagged plugin this is the same as for a regular one; no runtime (FE xxx) id is involved, so the files work in any load order.
- **Length**: words / 2.6 + 1 s, clamped to 2-12 s (Fuz Ro D'oh uses the same shape). A list of every file with its text and length is written to `build/voice-files.tsv`.
- **Format**: 16-bit PCM mono 44.1 kHz WAV, which the engine plays directly. Bethesda's own files are FUZ (xWMA audio + LIP lip-sync), but xWMA can only be produced with Microsoft's Windows-only xWMAEncode and LIP files with the Creation Kit, so they are not generated. Consequences: no lip movement (the lines are silent anyway), ~18 MB on disk (silence compresses to almost nothing in the archive). The Creation Kit wiki notes that loose WAV/XWM voice files occasionally fail to play for unknown reasons; if a line still flashes past for you, please report it, and Fuz Ro D'oh is a fallback.
- The validator fails the build if any response has no file, so a new line can't ship without one.

## SEQ file

`GMD_Intro` is Start Game Enabled and holds dialogue. Since 1.7 the engine expects such quests to be listed in `SEQ/<plugin>.seq` (what the Creation Kit writes on save and xEdit's "Create SEQ file" produces): a headerless array of little-endian FormIDs in the plugin's on-disk form, here `01000800` (master index 01, local id 800). The generator writes it from the finished plugin and the validator compares it against the quest list.

## Robustness

- **Eorlund dead**: the opening topic can't be offered by a dead man, and `GMD_Main`'s Eorlund alias has no *Allow Dead* flag, so the quest cannot start even if forced. If he dies mid-quest, the alias script fails the quest (stage 200) and it stops cleanly.
- **No free dungeon**: if every candidate is reserved by other quests, `GMD_Main` doesn't start, a message says so, and the topic stays available.
- **Civil War**: Eorlund stays at the Skyforge under either side. The opening is hidden during a siege; an active questline is unaffected by who holds Whiterun.
- **Companions / Missing in Action**: no shared records, aliases or conditions. The only Companions interaction is the C06 condition on the opening.
- **Hrodulf killed early** (at stage 30, before you return the pattern): counts as the fight ending; Eorlund takes the pattern back when you report.

## Scaled down (honest list)

- **No scenes or AI packages.** Eorlund doesn't walk anywhere or perform; everything happens in dialogue at his forge. Hrodulf and his enforcers have no packages and simply stand at the stables (enforcers inherit bandit packages and loiter). A Creation Kit scene with blocking would look better.
- **One investigation step.** The "investigation" is the thief's note plus one randomly picked dungeon. Asking around Whiterun would need new dialogue on other vanilla NPCs, each with their own voice type and files.
- **Consequences of betrayal are limited to this mod's dialogue.** Eorlund still sells and trains as usual; changing that would mean editing him.
- **WAV instead of FUZ, no lip sync** (see above).
- **The confrontation is at a fixed place**, the Whiterun Stables map marker, because there is no world edit to stage it anywhere bespoke. Guards and stablehands may join a fight there.
