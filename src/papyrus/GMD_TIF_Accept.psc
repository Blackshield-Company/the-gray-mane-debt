Scriptname GMD_TIF_Accept extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: accept Eorlund's request.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_IntroScript).Accept()
EndFunction
