Scriptname GMD_TIF_RewardPaid extends TopicInfo Hidden
{The Gray-Mane Debt - dialogue fragment: Eorlund's reward after the debt was paid.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as GMD_MainScript).RewardPaid()
EndFunction
