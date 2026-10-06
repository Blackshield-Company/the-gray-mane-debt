Scriptname GMD_TicketAliasScript extends ReferenceAlias
{The Gray-Mane Debt - on the Ticket alias. Reading the note reveals where the thief went.}

Event OnRead()
	Utility.Wait(0.1)
	(GetOwningQuest() as GMD_MainScript).TicketRead()
EndEvent
