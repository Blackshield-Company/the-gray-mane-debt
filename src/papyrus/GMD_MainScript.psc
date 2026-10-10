Scriptname GMD_MainScript extends Quest
{The Gray-Mane Debt - questline controller.
Stages: 10 read the ticket, 20 find the thief, 30 pattern recovered, 40 deal with Hrodulf,
50 debt paid, 60 Hrodulf dead, 100/110/120 completed (paid / fought / betrayed), 200 failed.}

ReferenceAlias Property Eorlund Auto
ReferenceAlias Property Pattern Auto
ReferenceAlias Property Creditor Auto
ReferenceAlias Property Thug1 Auto
ReferenceAlias Property Thug2 Auto
GlobalVariable Property GMD_State Auto
GlobalVariable Property GMD_DebtAmount Auto
GlobalVariable Property GMD_BetrayGold Auto
GlobalVariable Property GMD_SmithingLessons Auto
MiscObject Property Gold001 Auto
Weapon Property GMD_HeirloomBlade Auto
Faction Property GMD_CreditorFaction Auto
Faction Property PlayerFaction Auto
ReferenceAlias Property Thief Auto
ReferenceAlias Property ThiefBoss Auto
{Optional: the dungeon's boss spot. Brund is moved there at quest start.}
ReferenceAlias Property Letter Auto
MiscObject Property GMD_PatternItem Auto
Book Property GMD_LetterItem Auto

Bool PendingDismiss = False
Bool PendingStop = False

Function Begin()
	SetStage(10)
	PlaceThief()
	EnsurePattern()
	SetObjectiveDisplayed(10)
EndFunction

; 0.1.1: Brund was created at the dungeon's outdoor map marker.
Function PlaceThief()
	Actor b = Thief.GetReference() as Actor
	ObjectReference spot = ThiefBoss.GetReference()
	If b && spot
		b.MoveTo(spot)
		Debug.Trace("GMD_ Brund moved to the dungeon's boss spot")
	ElseIf b
		Debug.Trace("GMD_ no boss spot in this dungeon; Brund stays at the map marker")
	EndIf
EndFunction

; 0.1.1: make sure Brund carries the pattern and the letter (alias-created items could go missing).
; Called at quest start, when he loads, and when he dies. Safe to call any number of times.
Function EnsurePattern()
	Int st = GetStage()
	If st < 10 || st >= 30
		Return
	EndIf
	Actor player = Game.GetPlayer()
	If player.GetItemCount(GMD_PatternItem) > 0
		PatternTaken()
		Return
	EndIf
	Actor b = Thief.GetReference() as Actor
	If !b
		Debug.Trace("GMD_ EnsurePattern: no thief")
		Return
	EndIf
	If b.GetItemCount(GMD_PatternItem) == 0
		ObjectReference n = Pattern.GetReference()
		If !n
			n = b.PlaceAtMe(GMD_PatternItem, 1, True)
			Pattern.ForceRefTo(n)
		EndIf
		b.AddItem(n, 1, True)
		Debug.Trace("GMD_ EnsurePattern: pattern given to Brund")
	EndIf
	If b.GetItemCount(GMD_LetterItem) == 0 && player.GetItemCount(GMD_LetterItem) == 0
		ObjectReference l = Letter.GetReference()
		If !l
			l = b.PlaceAtMe(GMD_LetterItem, 1, True)
			Letter.ForceRefTo(l)
		EndIf
		b.AddItem(l, 1, True)
	EndIf
EndFunction

; Ticket alias: the player read the soot-stained note.
Function TicketRead()
	If GetStage() >= 10 && GetStage() < 20
		SetObjectiveCompleted(10)
		SetStage(20)
		SetObjectiveDisplayed(20)
	EndIf
EndFunction

; Pattern alias: the pattern moved into the player's inventory.
Function PatternTaken()
	If GetStage() < 10 || GetStage() >= 30
		Return
	EndIf
	SetObjectiveCompleted(10)
	SetObjectiveCompleted(20)
	SetStage(30)
	ShowCreditor(Creditor)
	ShowCreditor(Thug1)
	ShowCreditor(Thug2)
	SetObjectiveDisplayed(30)
	SetObjectiveDisplayed(31)
EndFunction

; Eorlund dialogue: "I have your pattern."
Function ReturnPattern()
	If GetStage() != 30
		Return
	EndIf
	TakePattern()
	SetObjectiveCompleted(30)
	SetObjectiveDisplayed(31, False)
	SetStage(40)
	SetObjectiveDisplayed(40)
EndFunction

; Hrodulf dialogue: sell him the pattern.
Function Betray()
	If GetStage() != 30
		Return
	EndIf
	TakePattern()
	Game.GetPlayer().AddItem(Gold001, GMD_BetrayGold.GetValueInt())
	SetObjectiveCompleted(31)
	SetObjectiveDisplayed(30, False)
	GMD_State.SetValue(3)
	SetStage(120)
	Finish(True)
EndFunction

; Hrodulf dialogue: pay the debt.
Function Pay()
	If GetStage() != 40
		Return
	EndIf
	Game.GetPlayer().RemoveItem(Gold001, GMD_DebtAmount.GetValueInt())
	SetObjectiveCompleted(40)
	SetStage(50)
	SetObjectiveDisplayed(50)
	PendingDismiss = True
	RegisterForSingleUpdate(4.0)
EndFunction

; Hrodulf dialogue: refuse and fight.
Function Fight()
	If GetStage() < 30 || GetStage() >= 50
		Return
	EndIf
	GMD_CreditorFaction.SetEnemy(PlayerFaction)
	Actor player = Game.GetPlayer()
	Attack(Creditor, player)
	Attack(Thug1, player)
	Attack(Thug2, player)
EndFunction

; Creditor alias OnDeath, whoever killed him.
Function CreditorDied()
	Int stage = GetStage()
	If stage < 30 || stage >= 50
		Return
	EndIf
	If stage == 40
		SetObjectiveCompleted(40)
	EndIf
	SetObjectiveDisplayed(31, False)
	SetStage(60)
	SetObjectiveDisplayed(60)
EndFunction

; Eorlund dialogue: "The debt is paid."
Function RewardPaid()
	If GetStage() != 50
		Return
	EndIf
	Actor player = Game.GetPlayer()
	player.AddItem(GMD_HeirloomBlade, 1)
	Int lessons = GMD_SmithingLessons.GetValueInt()
	While lessons > 0
		Game.IncrementSkill("Smithing")
		lessons -= 1
	EndWhile
	SetObjectiveCompleted(50)
	GMD_State.SetValue(2)
	SetStage(100)
	Finish(False)
EndFunction

; Eorlund dialogue: "Hrodulf is dead."
Function RewardFought()
	If GetStage() != 60
		Return
	EndIf
	If !GetStageDone(40)
		; Hrodulf died before the pattern went back to Eorlund: hand it over now.
		TakePattern()
		SetObjectiveCompleted(30)
	EndIf
	Game.GetPlayer().AddItem(GMD_HeirloomBlade, 1)
	SetObjectiveCompleted(60)
	GMD_State.SetValue(2)
	SetStage(110)
	Finish(False)
EndFunction

; Eorlund alias OnDeath.
Function EorlundDied()
	If GMD_State.GetValueInt() != 1 || GetStage() >= 100
		Return
	EndIf
	Debug.Trace("GMD_ Eorlund died at stage " + GetStage() + "; failing the quest")
	FailAllObjectives()
	GMD_State.SetValue(4)
	SetStage(200)
	Finish(True)
EndFunction

Function Finish(Bool abDismissCreditor)
	PendingDismiss = abDismissCreditor
	PendingStop = True
	RegisterForSingleUpdate(4.0)
EndFunction

Event OnUpdate()
	If PendingDismiss
		PendingDismiss = False
		Dismiss(Creditor)
		Dismiss(Thug1)
		Dismiss(Thug2)
	EndIf
	If PendingStop
		PendingStop = False
		Debug.Trace("GMD_ questline finished at stage " + GetStage() + ", state " + GMD_State.GetValueInt())
		Stop()
	EndIf
EndEvent

Function TakePattern()
	ObjectReference item = Pattern.GetReference()
	Actor player = Game.GetPlayer()
	If item && player.GetItemCount(item.GetBaseObject()) > 0
		player.RemoveItem(item.GetBaseObject(), 1, True)
	EndIf
EndFunction

Function ShowCreditor(ReferenceAlias akAlias)
	ObjectReference ref = akAlias.GetReference()
	If ref && ref.IsDisabled()
		ref.Enable()
	EndIf
EndFunction

Function Dismiss(ReferenceAlias akAlias)
	Actor a = akAlias.GetActorReference()
	If a && !a.IsDead() && !a.IsDisabled()
		a.Disable(True)
	EndIf
EndFunction

Function Attack(ReferenceAlias akAlias, Actor akTarget)
	Actor a = akAlias.GetActorReference()
	If a && !a.IsDead()
		a.SetActorValue("Aggression", 1.0)
		a.StartCombat(akTarget)
	EndIf
EndFunction
