Scriptname GMD_TIF_RewardFought extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: Eorlund's reward after Hrodulf died.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).RewardFought()
EndFunction
