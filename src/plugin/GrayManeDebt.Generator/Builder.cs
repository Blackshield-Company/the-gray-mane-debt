using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using static GrayManeDebt.Generator.Vanilla;

namespace GrayManeDebt.Generator;

internal sealed class Builder
{
    private readonly SkyrimMod _mod;
    private readonly FormIdRegistry _ids;
    private static readonly SkyrimRelease R = SkyrimRelease.SkyrimSE;
    private const byte VanillaQuestFormVersion = 65;

    // GMD_Main alias layout
    public const int APlayer = 0, AEorlund = 1, AThiefLocation = 2, AThiefMarker = 3, AThief = 4, APattern = 5,
        ALetter = 6, ATicket = 7, AMeetingMarker = 8, ACreditor = 9, AThug1 = 10, AThug2 = 11;

    public HashSet<string> ExpectedUnfilled { get; } = new();

    /// <summary>Who speaks each topic: (voice type folder, speaker label). Used for the silent voice files.</summary>
    public Dictionary<FormKey, (string VoiceType, string Speaker)> Speakers { get; } = new();

    public Builder(SkyrimMod mod, FormIdRegistry ids)
    {
        _mod = mod;
        _ids = ids;
    }

    private FormKey Id(string editorId) => _ids.Get(editorId);
    private static FormLink<T> L<T>(FormKey k) where T : class, IMajorRecordGetter => new(k);

    public void Build()
    {
        var qIntro = Id("GMD_Intro");
        var qMain = Id("GMD_Main");

        // --- Globals ---------------------------------------------------------------------------
        var gState = Global("GMD_State", 0);
        var gMinLevel = Global("GMD_MinLevel", 5);
        var gDebt = Global("GMD_DebtAmount", 1000);
        var gBetray = Global("GMD_BetrayGold", 2000);
        var gLessons = Global("GMD_SmithingLessons", 2);

        // --- Factions, lists ---------------------------------------------------------------------
        var creditorFaction = Add(new Faction(Id("GMD_CreditorFaction"), R) { EditorID = "GMD_CreditorFaction", Name = "Ash-Purse Crew" });
        var holds = Add(new FormList(Id("GMD_ThiefHolds"), R) { EditorID = "GMD_ThiefHolds" });
        foreach (var h in new[] { WhiterunHoldLocation, FalkreathHoldLocation, HjaalmarchHoldLocation, PaleHoldLocation, EastmarchHoldLocation })
            holds.Items.Add(L<ISkyrimMajorRecordGetter>(h));

        // --- Items -------------------------------------------------------------------------------
        var pattern = Add(new MiscItem(Id("GMD_SkyforgePattern"), R)
        {
            EditorID = "GMD_SkyforgePattern",
            Name = Texts.PatternName,
            ObjectBounds = Bounds(-3, -12, -3, 3, 12, 3),
            Model = new Model { File = MeshHorkerTusk },
            PickUpSound = L<ISoundDescriptorGetter>(ITMGenericUpSD).AsNullable(),
            PutDownSound = L<ISoundDescriptorGetter>(ITMGenericDownSD).AsNullable(),
            Keywords = new() { L<IKeywordGetter>(VendorItemClutter) },
            Value = 0,
            Weight = 0.5f,
        });
        var ticket = Note("GMD_ThiefNote", Texts.TicketName, Texts.Ticket, MeshNote01, InvArtNote01);
        var letter = Note("GMD_HrodulfLetter", Texts.LetterName, Texts.Letter, MeshNote02, InvArtNote02);
        var blade = Add(new Weapon(Id("GMD_HeirloomBlade"), R)
        {
            EditorID = "GMD_HeirloomBlade",
            Name = Texts.BladeName,
            ObjectBounds = Bounds(-48, -11, -1, 8, 70, 1),
            Model = new Model { File = MeshSteelSword },
            EquipmentType = L<IEquipTypeGetter>(EquipRightHand).AsNullable(),
            BlockBashImpact = L<IImpactDataSetGetter>(BlockBashImpact).AsNullable(),
            AlternateBlockMaterial = L<IMaterialTypeGetter>(AltBlockMaterialSteel).AsNullable(),
            ImpactDataSet = L<IImpactDataSetGetter>(ImpactDataSetSword).AsNullable(),
            FirstPersonModel = L<IStaticGetter>(FirstPersonSteelSword).AsNullable(),
            AttackFailSound = L<ISoundDescriptorGetter>(WeaponAttackFail).AsNullable(),
            EquipSound = L<ISoundDescriptorGetter>(WeaponEquip).AsNullable(),
            UnequipSound = L<ISoundDescriptorGetter>(WeaponUnequip).AsNullable(),
            ObjectEffect = L<IObjectEffectGetter>(EnchWeaponFrostDamage03).AsNullable(),
            EnchantmentAmount = 2000,
            Description = "",
            BasicStats = new WeaponBasicStats { Value = 650, Weight = 10, Damage = 13 },
            Data = new WeaponData
            {
                AnimationType = WeaponAnimationType.OneHandSword,
                Speed = 1, Reach = 1, NumProjectiles = 1, AnimationAttackMult = 1,
                Unknown2 = 1061997773,
                RumbleLeftMotorStrength = 0.5f, RumbleRightMotorStrength = 1, RumbleDuration = 0.33f,
                Skill = Skill.OneHanded, Stagger = 0.75f,
            },
            Critical = new CriticalData { Damage = 6, PercentMult = 1, Flags = CriticalData.Flag.OnDeath },
            DetectionSoundLevel = SoundLevel.Normal,
            Keywords = new() { L<IKeywordGetter>(WeapMaterialSteel), L<IKeywordGetter>(WeapTypeSword), L<IKeywordGetter>(VendorItemWeapon) },
        });

        // --- Actors ------------------------------------------------------------------------------
        var thiefFlags = NpcConfiguration.TemplateFlag.Traits | NpcConfiguration.TemplateFlag.Stats | NpcConfiguration.TemplateFlag.Factions
            | NpcConfiguration.TemplateFlag.SpellList | NpcConfiguration.TemplateFlag.AIData | NpcConfiguration.TemplateFlag.AIPackages
            | NpcConfiguration.TemplateFlag.ModelAnimation | NpcConfiguration.TemplateFlag.Inventory | NpcConfiguration.TemplateFlag.Script
            | NpcConfiguration.TemplateFlag.DefPackList | NpcConfiguration.TemplateFlag.AttackData | NpcConfiguration.TemplateFlag.Keywords;
        var thief = Add(TemplatedNpc("GMD_Thief", Texts.ThiefName, Texts.ThiefShort, LCharBanditMeleeNordM, thiefFlags));

        // Enforcers: bandit looks, gear and combat, but Hrodulf's faction and AI (not hostile until the fight).
        var thugFlags = thiefFlags & ~(NpcConfiguration.TemplateFlag.Factions | NpcConfiguration.TemplateFlag.AIData | NpcConfiguration.TemplateFlag.Script);
        var thug = Add(TemplatedNpc("GMD_Enforcer", Texts.ThugName, Texts.ThugShort, LCharBanditMeleeAny, thugFlags));
        thug.Factions.Add(new RankPlacement { Faction = L<IFactionGetter>(creditorFaction.FormKey), Rank = 0 });
        thug.AIData = Unaggressive();

        // Hrodulf: only Traits (face, race, voice) come from a vanilla guard look NPC, so his voice type is
        // known (MaleNordCommander) and the silent voice files can be placed for it.
        var creditor = Add(TemplatedNpc("GMD_Hrodulf", Texts.CreditorName, Texts.CreditorShort, EncGuardSonsM03MaleNordCommander, NpcConfiguration.TemplateFlag.Traits));
        creditor.Configuration.Level = new PcLevelMult { LevelMult = 1.1f };
        creditor.Configuration.CalcMinLevel = 8;
        creditor.Configuration.CalcMaxLevel = 45;
        creditor.CombatStyle = L<ICombatStyleGetter>(CsHumanBoss1H).AsNullable();
        creditor.DefaultOutfit = L<IOutfitGetter>(FineClothesOutfit02).AsNullable();
        creditor.Factions.Add(new RankPlacement { Faction = L<IFactionGetter>(creditorFaction.FormKey), Rank = 1 });
        creditor.Items = new ExtendedList<ContainerEntry>
        {
            new() { Item = new ContainerItem { Item = L<IItemGetter>(SteelSword), Count = 1 } },
            new() { Item = new ContainerItem { Item = L<IItemGetter>(Gold001), Count = 350 } },
        };
        creditor.AIData = Unaggressive();
        creditor.AIData.Confidence = Confidence.Brave;

        // --- Messages ----------------------------------------------------------------------------
        var msgTrailCold = Msg("GMD_MsgTrailCold", Texts.TrailCold);

        // --- Quests ------------------------------------------------------------------------------
        var intro = Add(new Quest(qIntro, R)
        {
            EditorID = "GMD_Intro",
            Name = Texts.QuestName,
            Flags = Quest.Flag.StartGameEnabled,
            Priority = 50,
            Type = Quest.TypeEnum.None,
            QuestFormVersion = VanillaQuestFormVersion,
        });
        intro.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("GMD_IntroScript",
            Vmad.Obj("GMD_Main", qMain), Vmad.Obj("GMD_State", gState), Vmad.Obj("GMD_MsgTrailCold", msgTrailCold)));

        var main = Add(new Quest(qMain, R)
        {
            EditorID = "GMD_Main",
            Name = Texts.QuestName,
            Priority = 60,
            Type = Quest.TypeEnum.SideQuest,
            QuestFormVersion = VanillaQuestFormVersion,
        });
        main.TextDisplayGlobals.Add(L<IGlobalGetter>(gDebt));
        Stage(main, 0, QuestStage.Flag.StartUpStage, null);
        Stage(main, 10, 0, Texts.Log10);
        Stage(main, 20, 0, Texts.Log20);
        Stage(main, 30, 0, Texts.Log30);
        Stage(main, 40, 0, Texts.Log40);
        Stage(main, 50, 0, Texts.Log50);
        Stage(main, 60, 0, Texts.Log60);
        Stage(main, 100, 0, Texts.Log100, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 110, 0, Texts.Log110, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 120, 0, Texts.Log120, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 200, 0, Texts.Log200, QuestLogEntry.Flag.FailQuest);
        Objective(main, 10, Texts.Obj10);
        Objective(main, 20, Texts.Obj20, AThief);
        Objective(main, 30, Texts.Obj30, AEorlund);
        Objective(main, 31, Texts.Obj31, ACreditor);
        Objective(main, 40, Texts.Obj40, ACreditor);
        Objective(main, 50, Texts.Obj50, AEorlund);
        Objective(main, 60, Texts.Obj60, AEorlund);

        main.Aliases.Add(new QuestAlias { ID = APlayer, Type = QuestAlias.TypeEnum.Reference, Name = "Player", ForcedReference = new FormLinkNullable<IPlacedGetter>(PlayerRef) });
        // Unique actor fill: no edit to Eorlund. Without AllowDead the quest cannot start if he is dead.
        main.Aliases.Add(new QuestAlias { ID = AEorlund, Type = QuestAlias.TypeEnum.Reference, Name = "Eorlund", UniqueActor = L<INpcGetter>(EorlundGrayMane).AsNullable() });
        var loc = new QuestAlias
        {
            ID = AThiefLocation, Type = QuestAlias.TypeEnum.Location, Name = "ThiefLocation",
            Flags = QuestAlias.Flag.ReservesLocationOrReference | QuestAlias.Flag.StoresText | QuestAlias.Flag.AllowCleared,
        };
        loc.Conditions.Add(Cond.InCurrentLocFormList(holds.FormKey));
        loc.Conditions.Add(Cond.LocationHasKeyword(LocTypeDungeon));
        loc.Conditions.Add(Cond.LocationHasRefType(RefTypeMapMarker));
        main.Aliases.Add(loc);
        main.Aliases.Add(new QuestAlias
        {
            ID = AThiefMarker, Type = QuestAlias.TypeEnum.Reference, Name = "ThiefMarker",
            Location = new LocationAliasReference { AliasID = AThiefLocation, RefType = L<ILocationReferenceTypeGetter>(RefTypeMapMarker).AsNullable() },
        });
        main.Aliases.Add(Created(AThief, "Thief", thief.FormKey, AThiefMarker, CreateReferenceToObject.CreateEnum.At, Level.Medium, QuestAlias.Flag.StoresText));
        main.Aliases.Add(Created(APattern, "Pattern", pattern.FormKey, AThief, CreateReferenceToObject.CreateEnum.In, Level.Easy, QuestAlias.Flag.QuestObject));
        main.Aliases.Add(Created(ALetter, "Letter", letter.FormKey, AThief, CreateReferenceToObject.CreateEnum.In, Level.Easy, 0));
        main.Aliases.Add(Created(ATicket, "Ticket", ticket.FormKey, APlayer, CreateReferenceToObject.CreateEnum.In, Level.Easy, QuestAlias.Flag.QuestObject));
        main.Aliases.Add(new QuestAlias { ID = AMeetingMarker, Type = QuestAlias.TypeEnum.Reference, Name = "MeetingMarker", ForcedReference = new FormLinkNullable<IPlacedGetter>(WhiterunStablesMapMarker) });
        main.Aliases.Add(Created(ACreditor, "Creditor", creditor.FormKey, AMeetingMarker, CreateReferenceToObject.CreateEnum.At, Level.Medium, QuestAlias.Flag.InitiallyDisabled | QuestAlias.Flag.StoresText));
        main.Aliases.Add(Created(AThug1, "Thug1", thug.FormKey, AMeetingMarker, CreateReferenceToObject.CreateEnum.At, Level.Medium, QuestAlias.Flag.InitiallyDisabled | QuestAlias.Flag.Optional));
        main.Aliases.Add(Created(AThug2, "Thug2", thug.FormKey, AMeetingMarker, CreateReferenceToObject.CreateEnum.At, Level.Medium, QuestAlias.Flag.InitiallyDisabled | QuestAlias.Flag.Optional));
        main.NextAliasID = (uint)main.Aliases.Count;

        main.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("GMD_MainScript",
                Vmad.Alias("Eorlund", qMain, AEorlund), Vmad.Alias("Pattern", qMain, APattern),
                Vmad.Alias("Creditor", qMain, ACreditor), Vmad.Alias("Thug1", qMain, AThug1), Vmad.Alias("Thug2", qMain, AThug2),
                Vmad.Obj("GMD_State", gState), Vmad.Obj("GMD_DebtAmount", gDebt), Vmad.Obj("GMD_BetrayGold", gBetray),
                Vmad.Obj("GMD_SmithingLessons", gLessons), Vmad.Obj("Gold001", Gold001), Vmad.Obj("GMD_HeirloomBlade", blade.FormKey),
                Vmad.Obj("GMD_CreditorFaction", creditorFaction.FormKey), Vmad.Obj("PlayerFaction", PlayerFaction)))
            .WithAliasScripts(qMain, AEorlund, Vmad.Script("GMD_EorlundAliasScript"))
            .WithAliasScripts(qMain, ACreditor, Vmad.Script("GMD_CreditorAliasScript"))
            .WithAliasScripts(qMain, APattern, Vmad.Script("GMD_PatternAliasScript"))
            .WithAliasScripts(qMain, ATicket, Vmad.Script("GMD_TicketAliasScript"));

        // --- Dialogue ----------------------------------------------------------------------------
        var eorlund = (Vanilla.EorlundVoiceType, "Eorlund Gray-Mane");
        var hrodulf = ("MaleNordCommander", Texts.CreditorName); // from EncGuardSonsM03MaleNordCommander
        var isEorlund = Cond.IsID(EorlundGrayMane);
        var isCreditor = Cond.IsAliasRef(ACreditor);
        Condition State(int v) => Cond.Global(gState, CompareOperator.EqualTo, v);
        Condition StageIs(CompareOperator op, int s) => Cond.Stage(qMain, op, s);
        var hasPattern = Cond.ItemCount(pattern.FormKey, CompareOperator.GreaterThanOrEqualTo, 1).OnRef(PlayerRef);

        // Opening, in the always-running GMD_Intro quest
        var bIntro = Id("GMD_IntroBranch");
        var tStart = Topic("GMD_I_Start", qIntro, bIntro, Texts.TopicStart, eorlund);
        var tAccept = Topic("GMD_I_Accept", qIntro, bIntro, Texts.TopicAccept, eorlund);
        var tDecline = Topic("GMD_I_Decline", qIntro, bIntro, Texts.TopicDecline, eorlund);
        Branch(bIntro, "GMD_IntroBranch", qIntro, tStart);
        Info(tStart, "GMD_I_StartOffer", Texts.Start, new[]
            {
                isEorlund, State(0),
                Cond.MakeVsGlobal(new GetLevelConditionData(), gMinLevel, CompareOperator.GreaterThanOrEqualTo).OnRef(PlayerRef),
                Cond.QuestRunning(C06, false), Cond.QuestRunning(CWSiegeObj, false), Cond.QuestRunning(qMain, false),
            },
            links: new[] { tAccept, tDecline });
        Info(tStart, "GMD_I_StartFriend", Texts.Friendly, new[] { isEorlund, State(2) }, prompt: Texts.PromptFriendly);
        Info(tStart, "GMD_I_StartCold", Texts.Cold, new[] { isEorlund, State(3) }, prompt: Texts.PromptCold, goodbye: true);
        Info(tAccept, "GMD_I_AcceptInfo", Texts.Accept, new[] { isEorlund }, goodbye: true, fragment: "GMD_TIF_Accept");
        Info(tDecline, "GMD_I_DeclineInfo", Texts.Decline, new[] { isEorlund }, goodbye: true);

        // Questline, Eorlund
        var bEorlund = Id("GMD_EorlundBranch");
        var tStatus = Topic("GMD_E_Status", qMain, bEorlund, Texts.TopicStatus, eorlund);
        Branch(bEorlund, "GMD_EorlundBranch", qMain, tStatus);
        Info(tStatus, "GMD_E_StatusNote", Texts.StatusNote, new[] { isEorlund, StageIs(CompareOperator.GreaterThanOrEqualTo, 10), StageIs(CompareOperator.LessThan, 20) });
        Info(tStatus, "GMD_E_StatusThief", Texts.StatusThief, new[] { isEorlund, StageIs(CompareOperator.GreaterThanOrEqualTo, 20), StageIs(CompareOperator.LessThan, 30) });
        Info(tStatus, "GMD_E_StatusDebt", Texts.StatusDebt, new[] { isEorlund, StageIs(CompareOperator.EqualTo, 40) });

        var bReturn = Id("GMD_ReturnBranch");
        var tReturn = Topic("GMD_E_Return", qMain, bReturn, Texts.TopicReturn, eorlund);
        Branch(bReturn, "GMD_ReturnBranch", qMain, tReturn);
        Info(tReturn, "GMD_E_ReturnInfo", Texts.Return, new[] { isEorlund, StageIs(CompareOperator.EqualTo, 30), hasPattern }, fragment: "GMD_TIF_ReturnPattern");

        var bPaid = Id("GMD_PaidBranch");
        var tPaid = Topic("GMD_E_Paid", qMain, bPaid, Texts.TopicPaid, eorlund);
        Branch(bPaid, "GMD_PaidBranch", qMain, tPaid);
        Info(tPaid, "GMD_E_PaidInfo", Texts.Paid, new[] { isEorlund, StageIs(CompareOperator.EqualTo, 50) }, goodbye: true, fragment: "GMD_TIF_RewardPaid");

        var bFought = Id("GMD_FoughtBranch");
        var tFought = Topic("GMD_E_Fought", qMain, bFought, Texts.TopicFought, eorlund);
        Branch(bFought, "GMD_FoughtBranch", qMain, tFought);
        var notReturned = new GetStageDoneConditionData { Stage = 40 };
        notReturned.Quest.Link.SetTo(qMain);
        Info(tFought, "GMD_E_FoughtPattern", Texts.FoughtWithPattern, new[] { isEorlund, StageIs(CompareOperator.EqualTo, 60), Cond.Make(notReturned, 0) }, goodbye: true, fragment: "GMD_TIF_RewardFought");
        Info(tFought, "GMD_E_FoughtInfo", Texts.Fought, new[] { isEorlund, StageIs(CompareOperator.EqualTo, 60) }, goodbye: true, fragment: "GMD_TIF_RewardFought");

        // Questline, Hrodulf
        var bCreditor = Id("GMD_CreditorBranch");
        var tHello = Topic("GMD_C_Hello", qMain, bCreditor, Texts.TopicHello, hrodulf);
        var tBetray = Topic("GMD_C_Betray", qMain, bCreditor, Texts.TopicBetray, hrodulf);
        var tPay = Topic("GMD_C_Pay", qMain, bCreditor, Texts.TopicPay, hrodulf);
        var tFight = Topic("GMD_C_Fight", qMain, bCreditor, Texts.TopicFight, hrodulf);
        var tLeave = Topic("GMD_C_Leave", qMain, bCreditor, Texts.TopicLeave, hrodulf);
        Branch(bCreditor, "GMD_CreditorBranch", qMain, tHello);
        Info(tHello, "GMD_C_HelloInfo", Texts.Hello, new[] { isCreditor, StageIs(CompareOperator.GreaterThanOrEqualTo, 30), StageIs(CompareOperator.LessThan, 50) },
            links: new[] { tBetray, tPay, tFight, tLeave });
        Info(tBetray, "GMD_C_BetrayInfo", Texts.Betray, new[] { isCreditor, StageIs(CompareOperator.EqualTo, 30), hasPattern }, goodbye: true, fragment: "GMD_TIF_Betray");
        var goldEnough = Cond.MakeVsGlobal(Cond.ItemCountData(Gold001), gDebt, CompareOperator.GreaterThanOrEqualTo).OnRef(PlayerRef);
        var goldShort = Cond.MakeVsGlobal(Cond.ItemCountData(Gold001), gDebt, CompareOperator.LessThan).OnRef(PlayerRef);
        Info(tPay, "GMD_C_PayInfo", Texts.Pay, new[] { isCreditor, StageIs(CompareOperator.EqualTo, 40), goldEnough }, goodbye: true, fragment: "GMD_TIF_Pay");
        Info(tPay, "GMD_C_PayShort", Texts.PayShort, new[] { isCreditor, StageIs(CompareOperator.EqualTo, 40), goldShort });
        Info(tFight, "GMD_C_FightInfo", Texts.Fight, new[] { isCreditor, StageIs(CompareOperator.GreaterThanOrEqualTo, 30), StageIs(CompareOperator.LessThan, 50) }, goodbye: true, fragment: "GMD_TIF_Fight");
        Info(tLeave, "GMD_C_LeaveInfo", Texts.Leave, new[] { isCreditor }, goodbye: true);
    }

    // ----------------------------------------------------------------------------------------
    // Dialogue helpers
    // ----------------------------------------------------------------------------------------

    private void Branch(FormKey fk, string edid, FormKey quest, DialogTopic starting)
        => Add(new DialogBranch(fk, R)
        {
            EditorID = edid,
            Quest = L<IQuestGetter>(quest),
            Category = DialogBranch.CategoryType.Player,
            Flags = DialogBranch.Flag.TopLevel,
            StartingTopic = L<IDialogTopicGetter>(starting.FormKey).AsNullable(),
        });

    private DialogTopic Topic(string edid, FormKey quest, FormKey branch, string text, (string VoiceType, string Speaker) speaker)
    {
        var t = Add(new DialogTopic(Id(edid), R)
        {
            EditorID = edid,
            Name = text,
            Priority = 50,
            Branch = L<IDialogBranchGetter>(branch).AsNullable(),
            Quest = L<IQuestGetter>(quest).AsNullable(),
            Category = DialogTopic.CategoryEnum.Topic,
            Subtype = DialogTopic.SubtypeEnum.Custom,
            SubtypeName = new RecordType("CUST"),
        });
        Speakers[t.FormKey] = speaker;
        return t;
    }

    private void Info(DialogTopic topic, string edid, string[] lines, Condition[] conditions, string? prompt = null,
        bool goodbye = false, string? fragment = null, DialogTopic[]? links = null)
    {
        var info = new DialogResponses(Id(edid), R)
        {
            EditorID = edid,
            Flags = new DialogResponseFlags { Flags = goodbye ? DialogResponses.Flag.Goodbye : 0 },
            PreviousDialog = topic.Responses.Count == 0 ? new FormLinkNullable<IDialogResponsesGetter>() : L<IDialogResponsesGetter>(topic.Responses[^1].FormKey).AsNullable(),
        };
        if (prompt != null) info.Prompt = prompt;
        for (int i = 0; i < lines.Length; i++)
            info.Responses.Add(new DialogResponse { Emotion = Emotion.Neutral, EmotionValue = 50, ResponseNumber = (byte)(i + 1), Text = lines[i] });
        info.Conditions.AddRange(conditions);
        if (links != null)
            foreach (var l in links) info.LinkTo.Add(L<IDialogTopicGetter>(l.FormKey));
        if (fragment != null) info.VirtualMachineAdapter = Vmad.InfoFragment(fragment);
        topic.Responses.Add(info);
    }

    // ----------------------------------------------------------------------------------------
    // Record helpers
    // ----------------------------------------------------------------------------------------

    private T Add<T>(T record) where T : SkyrimMajorRecord
    {
        _mod.GetTopLevelGroup<T>().Add(record);
        return record;
    }

    private FormKey Global(string edid, float value)
    {
        var g = new GlobalFloat(Id(edid), R) { EditorID = edid, Data = value };
        _mod.Globals.Add(g);
        return g.FormKey;
    }

    private static void Stage(Quest q, ushort index, QuestStage.Flag flags, string? log, QuestLogEntry.Flag logFlags = 0)
    {
        var s = new QuestStage { Index = index, Flags = flags };
        if (log != null) s.LogEntries.Add(new QuestLogEntry { Flags = logFlags, Entry = log });
        q.Stages.Add(s);
    }

    private static void Objective(Quest q, ushort index, string text, int? target = null)
    {
        var o = new QuestObjective { Index = index, DisplayText = text };
        if (target is int t) o.Targets.Add(new QuestObjectiveTarget { AliasID = t });
        q.Objectives.Add(o);
    }

    private static QuestAlias Created(int id, string name, FormKey obj, int at, CreateReferenceToObject.CreateEnum how, Level level, QuestAlias.Flag flags)
        => new()
        {
            ID = (uint)id, Type = QuestAlias.TypeEnum.Reference, Name = name,
            Flags = flags == 0 ? null : flags,
            CreateReferenceToObject = new CreateReferenceToObject { Object = L<ISkyrimMajorRecordGetter>(obj), AliasID = (short)at, Create = how, Level = level },
        };

    private Book Note(string edid, string name, string text, string mesh, FormKey invArt) => Add(new Book(Id(edid), R)
    {
        EditorID = edid,
        Name = name,
        ObjectBounds = Bounds(-8, -5, 0, 8, 5, 0),
        Model = new Model { File = mesh },
        BookText = text,
        PickUpSound = L<ISoundDescriptorGetter>(ITMNoteUp).AsNullable(),
        Keywords = new() { L<IKeywordGetter>(VendorItemBook) },
        Type = Book.BookType.NoteOrScroll,
        Teaches = new BookTeachesNothing { RawContent = 0xFFFFFFFF },
        Value = 0,
        Weight = 0,
        InventoryArt = L<IStaticGetter>(invArt).AsNullable(),
        Description = "",
    });

    private static ObjectBounds Bounds(short x1, short y1, short z1, short x2, short y2, short z2)
        => new() { First = new P3Int16(x1, y1, z1), Second = new P3Int16(x2, y2, z2) };

    private static AIData Unaggressive() => new()
    {
        Aggression = Aggression.Unaggressive, Confidence = Confidence.Average, Assistance = Assistance.HelpsAllies, EnergyLevel = 50,
    };

    private FormKey Msg(string edid, string text)
    {
        var m = new Message(Id(edid), R) { EditorID = edid, Description = text, Flags = Message.Flag.MessageBox, INAM = new byte[4] };
        _mod.Messages.Add(m);
        return m.FormKey;
    }

    private Npc TemplatedNpc(string edid, string name, string shortName, FormKey template, NpcConfiguration.TemplateFlag flags) => new(Id(edid), R)
    {
        EditorID = edid,
        Name = name,
        ShortName = shortName,
        ObjectBounds = Bounds(-22, -14, 0, 22, 14, 128),
        Template = new FormLinkNullable<INpcSpawnGetter>(template),
        Race = L<IRaceGetter>(NordRace),
        Class = L<IClassGetter>(ClassWarrior1H),
        CombatStyle = L<ICombatStyleGetter>(CsHumanMelee).AsNullable(),
        Configuration = new NpcConfiguration
        {
            Flags = NpcConfiguration.Flag.AutoCalcStats,
            TemplateFlags = flags,
            Level = new PcLevelMult { LevelMult = 1 },
            CalcMinLevel = 1,
            CalcMaxLevel = 0,
            SpeedMultiplier = 100,
            DispositionBase = 35,
        },
        AIData = new AIData { Aggression = Aggression.Aggressive, Confidence = Confidence.Average, Assistance = Assistance.HelpsAllies, EnergyLevel = 50 },
        PlayerSkills = new PlayerSkills(),
        Height = 1,
        Weight = 50,
    };
}
