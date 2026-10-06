Scriptname GMD_CreditorAliasScript extends ReferenceAlias
{The Gray-Mane Debt - on the Creditor alias (Hrodulf Ash-Purse). His death ends the debt.}

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as GMD_MainScript).CreditorDied()
EndEvent
