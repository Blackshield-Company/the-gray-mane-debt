Scriptname GMD_PatternAliasScript extends ReferenceAlias
{The Gray-Mane Debt - on the Pattern alias. Advances the quest when the player takes the pattern.}

Event OnContainerChanged(ObjectReference akNewContainer, ObjectReference akOldContainer)
	If akNewContainer == Game.GetPlayer()
		(GetOwningQuest() as GMD_MainScript).PatternTaken()
	EndIf
EndEvent
