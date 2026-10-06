Scriptname GMD_TIF_ReturnPattern extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: hand the pattern back to Eorlund.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).ReturnPattern()
EndFunction
