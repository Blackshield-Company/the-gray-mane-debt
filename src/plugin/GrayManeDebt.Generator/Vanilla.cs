using Mutagen.Bethesda.Plugins;

namespace GrayManeDebt.Generator;

/// <summary>
/// Vanilla Skyrim.esm records this mod links to. Only FormIDs are referenced; no Bethesda data or
/// assets are copied. FormIDs were looked up with the Mutagen.Bethesda.FormKeys.SkyrimSE package.
/// None of these records is overridden.
/// </summary>
internal static class Vanilla
{
    public static readonly ModKey Skyrim = ModKey.FromFileName("Skyrim.esm");
    private static FormKey K(uint id) => new(Skyrim, id);

    public static readonly FormKey PlayerRef = K(0x000014);
    public static readonly FormKey PlayerFaction = K(0x000DB1);

    // Eorlund Gray-Mane (NPC_ base). Voice type 0E5003 MaleNordCommander, read from his NPC record.
    public static readonly FormKey EorlundGrayMane = K(0x013B9D);
    public const string EorlundVoiceType = "MaleNordCommander";

    // Quests the opening topic stays out of the way of
    public static readonly FormKey C06 = K(0x01CEF6);          // Companions: Glory of the Dead (Eorlund at the Skyforge funeral)
    public static readonly FormKey CWSiegeObj = K(0x096E71);   // Civil War: city siege in progress

    // Actors
    public static readonly FormKey NordRace = K(0x013746);
    public static readonly FormKey ClassWarrior1H = K(0x013176);
    public static readonly FormKey CsHumanMelee = K(0x068849);
    public static readonly FormKey CsHumanBoss1H = K(0x03DECE);
    public static readonly FormKey LCharBanditMeleeNordM = K(0x01A320);
    public static readonly FormKey LCharBanditMeleeAny = K(0x03DECD);
    // Traits template for Hrodulf: a vanilla guard look NPC with its own face and the MaleNordCommander voice type
    public static readonly FormKey EncGuardSonsM03MaleNordCommander = K(0x0AA935);
    public static readonly FormKey FineClothesOutfit02 = K(0x0E40DD);
    public static readonly FormKey SteelSword = K(0x013989);

    // Places
    public static readonly FormKey WhiterunStablesMapMarker = K(0x072879);
    public static readonly FormKey WhiterunHoldLocation = K(0x016772);
    public static readonly FormKey FalkreathHoldLocation = K(0x01676F);
    public static readonly FormKey HjaalmarchHoldLocation = K(0x01676E);
    public static readonly FormKey PaleHoldLocation = K(0x01676D);
    public static readonly FormKey EastmarchHoldLocation = K(0x01676A);
    public static readonly FormKey LocTypeDungeon = K(0x0130DB);
    public static readonly FormKey RefTypeMapMarker = K(0x10F63C);

    // Items, keywords, sounds, art
    public static readonly FormKey Gold001 = K(0x00000F);
    public static readonly FormKey VendorItemBook = K(0x0937A2);
    public static readonly FormKey VendorItemClutter = K(0x0914E9);
    public static readonly FormKey ITMNoteUp = K(0x0C7A54);
    public static readonly FormKey ITMGenericUpSD = K(0x03C7BA);
    public static readonly FormKey ITMGenericDownSD = K(0x03C7B9);
    public static readonly FormKey InvArtNote01 = K(0x097788);
    public static readonly FormKey InvArtNote02 = K(0x01541B);

    // Heirloom blade: structural references of the vanilla Skyforge Steel Sword (09F25C)
    public static readonly FormKey WeapMaterialSteel = K(0x01E719);
    public static readonly FormKey WeapTypeSword = K(0x01E711);
    public static readonly FormKey VendorItemWeapon = K(0x08F958);
    public static readonly FormKey EquipRightHand = K(0x013F42);
    public static readonly FormKey BlockBashImpact = K(0x0183FF);
    public static readonly FormKey AltBlockMaterialSteel = K(0x0774C2);
    public static readonly FormKey ImpactDataSetSword = K(0x013CAC);
    public static readonly FormKey FirstPersonSteelSword = K(0x0140EF);
    public static readonly FormKey WeaponAttackFail = K(0x03C730);
    public static readonly FormKey WeaponEquip = K(0x03C72E);
    public static readonly FormKey WeaponUnequip = K(0x03C72F);
    public static readonly FormKey EnchWeaponFrostDamage03 = K(0x045C39);

    // Meshes (by path)
    public const string MeshNote01 = @"Clutter\Books\Note01.nif";
    public const string MeshNote02 = @"Clutter\Books\Note02.nif";
    public const string MeshHorkerTusk = @"Clutter\HorkerTusk\HorkerTusk.nif";
    public const string MeshSteelSword = @"Weapons\Steel\SteelSword.nif";
}
