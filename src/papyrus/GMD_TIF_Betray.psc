Scriptname GMD_TIF_Betray extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: sell the pattern to Hrodulf.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).Betray()
EndFunction
