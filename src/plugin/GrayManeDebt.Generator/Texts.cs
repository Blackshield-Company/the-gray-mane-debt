namespace GrayManeDebt.Generator;

/// <summary>All player-facing text. Every dialogue response stays within the Creation Kit's 149 characters.</summary>
internal static class Texts
{
    public const string QuestName = "The Gray-Mane Debt";
    public const string ThiefName = "Brund Coal-Hand", ThiefShort = "Brund";
    public const string CreditorName = "Hrodulf Ash-Purse", CreditorShort = "Hrodulf";
    public const string ThugName = "Ash-Purse Enforcer", ThugShort = "Enforcer";

    // --- Opening (GMD_Intro, Eorlund) ---------------------------------------------------------
    public const string TopicStart = "You look troubled, Eorlund.";
    public static readonly string[] Start =
    {
        "Troubled? Hmph. Someone broke into the Skyforge three nights past. Took nothing but a strip of old horker ivory.",
        "My grandfather's tempering pattern. Every fold of Skyforge steel is counted off those notches. Without it, I'm guessing.",
        "And I know who's behind it. A debt my family owes. Will you help an old smith get it back?",
    };
    public const string PromptFriendly = "How's the forge, Eorlund?";
    public static readonly string[] Friendly =
    {
        "The fire burns clean and the folds come true. The Gray-Manes owe you, friend. That's one debt I don't mind carrying.",
    };
    public const string PromptCold = "Eorlund...";
    public static readonly string[] Cold =
    {
        "You sold my grandfather's pattern to a moneylender. Take your coin and get away from my forge.",
    };
    public const string TopicAccept = "I'll get your pattern back.";
    public static readonly string[] Accept =
    {
        "Good. The thief dropped this when he ran. A note, soot all over it. Read it; it may tell you where he went to ground.",
    };
    public const string TopicDecline = "Not my concern.";
    public static readonly string[] Decline = { "Then I've said too much. Forget I spoke." };

    // --- Questline (GMD_Main, Eorlund) --------------------------------------------------------
    public const string TopicStatus = "About the stolen pattern...";
    public static readonly string[] StatusNote = { "Read that note. It's the only trail we have, and trails go cold." };
    public static readonly string[] StatusThief = { "So the thief went to ground at <Alias=ThiefLocation>. Go carefully. Desperate men bite." };
    public static readonly string[] StatusDebt = { "Hrodulf waits by the stables like a crow on a fence post. Pay him or put him down. Either way, end it." };

    public const string TopicRecover = "Brund didn't have the pattern. Just this letter.";
    public static readonly string[] Recover = { "Let me see that. Hm. Wrapped in the letter's fold, there. He must have hidden it. Take it, and bring it back to me." };
    public const string TopicReturn = "I recovered your pattern.";
    public static readonly string[] Return =
    {
        "The ivory! Let me see... yes. Every notch where my grandfather cut it. You have my thanks.",
        "You've earned the truth. The winter my sons marched off, the forge went cold. I borrowed from Hrodulf Ash-Purse to buy coal and ore.",
        "I've paid him back nothing but excuses. He took the pattern as surety. Next time he'll take worse.",
        "He's camped by the Whiterun stables. Pay him what I owe, or make him forget the debt. I'll leave the how of it to you.",
    };

    public const string TopicPaid = "The debt is paid. Hrodulf is gone.";
    public static readonly string[] Paid =
    {
        "Paid? From your own purse? By Ysgramor... the Gray-Manes don't forget a thing like that.",
        "Take this blade. My father forged it, and it's the best thing in my house. Now come, let me show you how the folds are counted.",
    };

    public const string TopicFought = "Hrodulf is dead.";
    public static readonly string[] FoughtWithPattern =
    {
        "And you've brought my pattern home as well. The ivory and the debt, both settled in one night.",
        "Dead, though? Hmph. I'd not have chosen it, but I'll not weep for a man who stole from the Skyforge.",
        "Take this blade. My father forged it. It has served my family; let it serve you.",
    };
    public static readonly string[] Fought =
    {
        "Dead? Hmph. I'd not have chosen it, but I'll not weep for a man who stole from the Skyforge.",
        "Take this blade. My father forged it. It has served my family; let it serve you.",
    };

    // --- Questline (GMD_Main, Hrodulf) --------------------------------------------------------
    public const string TopicHello = "You're Hrodulf Ash-Purse.";
    public static readonly string[] Hello =
    {
        "And you're the errand-runner the old smith sent. Or someone with better sense. Speak quickly; my time costs coin.",
    };
    public const string TopicBetray = "I have the Skyforge pattern. What's it worth to you?";
    public static readonly string[] Betray = { "A fat purse, and Eorlund never learns who sold it. Pleasure doing business." };
    public const string TopicPay = "I'm here to settle the Gray-Mane debt in full.";
    public static readonly string[] Pay = { "Coin is coin, whoever's purse it comes from. The Gray-Manes are free of me. Tell the old man I said so." };
    public static readonly string[] PayShort = { "Then show me the coin. Words don't pay debts." };
    public const string TopicFight = "The Gray-Manes owe you nothing. Draw your blade.";
    public static readonly string[] Fight = { "Brave. Stupid, but brave. Lads, teach this one what a debt costs!" };
    public const string TopicLeave = "I'll be back.";
    public static readonly string[] Leave = { "I'll be here. Debts don't sleep." };

    // --- Journal ------------------------------------------------------------------------------
    public const string Log10 = "Eorlund Gray-Mane told me a thief broke into the Skyforge and stole his grandfather's tempering pattern, a strip of notched horker ivory. He gave me a soot-stained note the thief dropped.";
    public const string Log20 = "The note was meant for a thief named Brund Coal-Hand. He is lying low at <Alias=ThiefLocation> until the guards stop asking questions.";
    public const string Log30 = "I recovered the Skyforge pattern from Brund Coal-Hand. A letter on him says a moneylender, Hrodulf Ash-Purse, is waiting at the Whiterun Stables to buy it.";
    public const string Log40 = "Eorlund confessed that he owes Hrodulf Ash-Purse <Global=GMD_DebtAmount> gold, borrowed to keep the Skyforge lit. Hrodulf stole the pattern as surety. I can pay the debt, or settle it another way.";
    public const string Log50 = "I paid Hrodulf Ash-Purse the Gray-Mane debt. I should let Eorlund know.";
    public const string Log60 = "Hrodulf Ash-Purse is dead, and the Gray-Mane debt with him. I should tell Eorlund.";
    public const string Log100 = "I paid the Gray-Mane debt out of my own purse. Eorlund gave me his father's blade and showed me how Skyforge steel is folded.";
    public const string Log110 = "Hrodulf Ash-Purse is dead and the Skyforge pattern is home. Eorlund gave me his father's blade.";
    public const string Log120 = "I sold Eorlund's tempering pattern to Hrodulf Ash-Purse for a fat purse. The Gray-Manes will not forget it.";
    public const string Log200 = "Eorlund Gray-Mane is dead. Whatever his family owed, the debt is buried with him.";

    public const string Obj10 = "Read the thief's note";
    public const string Obj20 = "Recover the Skyforge pattern from Brund Coal-Hand at <Alias=ThiefLocation>";
    public const string Obj30 = "Return the Skyforge pattern to Eorlund";
    public const string Obj31 = "(Optional) Sell the pattern to Hrodulf Ash-Purse at the Whiterun Stables";
    public const string Obj40 = "Settle the Gray-Mane debt with Hrodulf Ash-Purse (<Global=GMD_DebtAmount> gold), or settle it with steel";
    public const string Obj50 = "Tell Eorlund the debt is paid";
    public const string Obj60 = "Tell Eorlund that Hrodulf is dead";

    // --- Items --------------------------------------------------------------------------------
    public const string TicketName = "Soot-Stained Note";
    public const string Ticket =
        "Brund,\n\nYou were seen leaving the Skyforge. Don't bring it to me yet. Go to ground at <Alias=ThiefLocation> until the guards stop asking, then bring the ivory to the stables.\n\nNobody pays for a thief in a cell.\n\n- H.";
    public const string LetterName = "Hrodulf's Letter";
    public const string Letter =
        "Brund,\n\nWhen the ivory is safe, bring it to me at the Whiterun Stables. My lads and I will wait there until the old man pays or the pattern is sold.\n\nAnd Brund: if Eorlund sends anyone sniffing after you and they live, I'll buy the pattern from them instead. A purse is a purse.\n\n- Hrodulf Ash-Purse";
    public const string PatternName = "Skyforge Tempering Pattern";
    public const string BladeName = "Gray-Mane Heirloom";
    public const string TrailCold = "Eorlund shakes his head. \"Not now. Ask me again another day.\"\n\n(The questline could not start: Eorlund must be alive and there must be a dungeon free for the thief to hide in. Nothing has changed; you can try again later.)";
}
