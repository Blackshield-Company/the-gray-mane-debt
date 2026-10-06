Scriptname GMD_TIF_Pay extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: pay Hrodulf the debt.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).Pay()
EndFunction
