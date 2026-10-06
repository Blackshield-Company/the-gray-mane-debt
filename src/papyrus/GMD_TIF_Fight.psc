Scriptname GMD_TIF_Fight extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: refuse Hrodulf and fight.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).Fight()
EndFunction
