Scriptname GMD_LetterAliasScript extends ReferenceAlias
{The Gray-Mane Debt - on Hrodulf's letter. 0.1.2: taking it from Brund makes sure the player ends up with the pattern.}

Event OnContainerChanged(ObjectReference akNewContainer, ObjectReference akOldContainer)
	If akNewContainer == Game.GetPlayer()
		(GetOwningQuest() as GMD_MainScript).LetterTaken()
	EndIf
EndEvent
