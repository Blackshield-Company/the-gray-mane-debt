Scriptname GMD_IntroScript extends Quest
{The Gray-Mane Debt - always-running dialogue quest. Holds Eorlund's opening topic and
starts the questline (GMD_Main) when the player accepts.}

Quest Property GMD_Main Auto
GlobalVariable Property GMD_State Auto
{0 = not started, 1 = running, 2 = finished (helped Eorlund), 3 = finished (betrayed him), 4 = failed}
Message Property GMD_MsgTrailCold Auto

; Called from the "I'll get your pattern back." dialogue fragment.
Function Accept()
	If GMD_State.GetValueInt() != 0 || GMD_Main.IsRunning()
		Debug.Trace("GMD_ Accept ignored: state " + GMD_State.GetValueInt())
		Return
	EndIf
	If GMD_Main.Start()
		GMD_State.SetValue(1)
		(GMD_Main as GMD_MainScript).Begin()
		Debug.Trace("GMD_ questline started")
	Else
		; Every alias must fill (Eorlund alive, a free dungeon for the thief). If not, nothing
		; has changed and the topic stays available for another try.
		Debug.Trace("GMD_ GMD_Main failed to start (Eorlund dead or no free dungeon for the thief)")
		GMD_MsgTrailCold.Show()
	EndIf
EndFunction
