# In-game test checklist

The build verifies the plugin, scripts, dialogue structure, voice files and SEQ automatically, but none of that proves the quest plays well. This is the checklist for a first playtest. Turn on **dialogue subtitles** and Papyrus logging:

```ini
; Skyrim.ini (or SkyrimCustom.ini), [Papyrus]
bEnableLogging=1
bEnableTrace=1
bLoadDebugInformation=1
```

Logs go to `Documents/My Games/Skyrim Special Edition/Logs/Script/Papyrus.0.log`. Every line from this mod starts with `GMD_`.

FormIDs: `help GMD_ 4` in the console lists this mod's records with their load-order FormIDs (ESL-flagged, so they look like `FExxx8xx`).

## Setup

- [ ] New game or existing save; level 5 or higher (`player.setlevel 10`).
- [ ] Go to Eorlund: `player.moveto 1A683` (EorlundGrayManeREF). He is at the Skyforge by day.

## Opening

- [ ] "You look troubled, Eorlund." is in his topic list. His three lines stay on screen for several seconds each (silent voice files working).
- [ ] "Not my concern." ends the conversation; the topic is still there next time.
- [ ] "I'll get your pattern back." → Soot-Stained Note added, journal *The Gray-Mane Debt* stage 10, objective "Read the thief's note". Log: `GMD_ questline started`.
- [ ] Below level 5 (`set GMD_MinLevel to 99`) the topic is hidden.

## Stages

- [ ] Reading the note → stage 20; the objective names a dungeon in Whiterun/Falkreath/Hjaalmarch/Pale/Eastmarch, with a map marker on Brund.
- [ ] Brund Coal-Hand is at the dungeon's boss spot (entrance if none), hostile. Papyrus log shows 'GMD_ Brund moved' or 'GMD_ no boss spot'. Looting him gives the pattern and Hrodulf's Letter → stage 30. (0.1.2: if the pattern is missing, taking the letter alone gives it to you → stage 30; or ask Eorlund "Brund didn't have the pattern".) Two objectives: return to Eorlund, (optional) sell to Hrodulf.
- [ ] Hrodulf and two enforcers now stand at the Whiterun Stables, not hostile.
- [ ] The pattern can't be dropped (quest item).
- [ ] Eorlund: "About the stolen pattern..." gives a status line at stages 10, 20 and 40.

## Endings (use a save at stage 30 for each)

- [ ] **Betray**: talk to Hrodulf → "I have the Skyforge pattern..." → pattern removed, 2000 gold, quest completed, Hrodulf and men fade out. Eorlund now only offers "Eorlund..." with a cold goodbye line.
- [ ] **Pay**: give Eorlund the pattern ("I recovered your pattern." → four lines) → stage 40. With less than 1000 gold Hrodulf says "Then show me the coin." With 1000+: gold removed, stage 50, Hrodulf leaves. Eorlund: "The debt is paid." → Gray-Mane Heirloom, Smithing +2 levels, quest completed. Eorlund then offers "How's the forge, Eorlund?".
- [ ] **Fight**: at stage 40, "The Gray-Manes owe you nothing..." → Hrodulf and enforcers attack. Killing Hrodulf → stage 60. Eorlund: "Hrodulf is dead." → Heirloom, completed. No bounty for killing them after they attacked.
- [ ] **Fight early**: at stage 30 (pattern still with you), pick the fight option → after Hrodulf dies, Eorlund's reply mentions the pattern and takes it.

## Failure and edge cases

- [ ] Eorlund dies mid-quest (`prid 1A683`, `kill`) → journal: quest failed, objectives failed, log `GMD_ Eorlund died at stage ...`, the stables crew disappears a few seconds later.
- [ ] Dead Eorlund before starting → topic never offered (`startquest GMD_Main` from the console also fails).
- [ ] During *Glory of the Dead* (`C06`) the opening topic is hidden.
- [ ] *Missing in Action* active at the same time: both quests progress normally.

## What to report

The stage (`sqv GMD_Main`), the outcome you expected, and the `GMD_` lines from the Papyrus log. For lines that flash past, the topic text and your voice-related mods (Fuz Ro D'oh, anything that edits Eorlund).
