Scriptname GMD_ThiefAliasScript extends ReferenceAlias
{The Gray-Mane Debt - on Brund Coal-Hand. 0.1.1: re-checks that he carries the pattern.}

Event OnLoad()
	(GetOwningQuest() as GMD_MainScript).EnsurePattern()
EndEvent

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as GMD_MainScript).EnsurePattern()
EndEvent
