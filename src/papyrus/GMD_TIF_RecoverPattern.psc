Scriptname GMD_TIF_RecoverPattern extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment (0.1.2): Brund had no pattern; Eorlund finds it in the letter.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).GivePatternToPlayer()
EndFunction
