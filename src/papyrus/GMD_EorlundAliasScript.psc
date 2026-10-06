Scriptname GMD_EorlundAliasScript extends ReferenceAlias
{The Gray-Mane Debt - on the Eorlund alias. If he dies mid-quest the questline fails cleanly.}

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as GMD_MainScript).EorlundDied()
EndEvent
