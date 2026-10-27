using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Armor.ArmorBonus;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.UI;
using LAP.Core.MiscDate;
using LAP.Core.NetCode.NetUtilities;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CIKeybinds.AstralArcanumUIHotkey.JustPressed && AstralArcanumRegen && !LAPInfo.AnyBossHere)
            {
                AstralArcanumUI.Toggle();
            }
            if (CIKeybinds.AegisHotKey.JustPressed)
            {
                if (ElysianAegis)
                {
                    ElysianGuard = !ElysianGuard;
                }
            }
            if (CIKeybinds.ArmorSetBonusHotKey.JustPressed)
            {
                if (BloodflareRanged)
                    BloodflareBonus.RangedArmorSetBonus_KeyPress(this, Player);
                if (TarragonMelee)
                    TarragonBonus.MeleeArmorSetBonus_KeyPress(this, Player);
                if (TarragonRogue)
                    TarragonBonus.Rogue_KeyPress(this, Player);
            }
            if (CIKeybinds.GodSlayerDash.JustPressed && GodSlayerDash && !Player.HasCD<GodSlayerDashLegacyCD>())
            {
                Player.ImmediatelyDash(LAPContent.DashType<GodSlayerDashLegacy>());
                Player.SyncedDash(LAPContent.DashType<GodSlayerDashLegacy>());
                if (LAPUtilities.IsLocalPlayer(Player.whoAmI))
                    Player.AddCD(LAPContent.CDType<GodSlayerDashLegacyCD>(), 60 * 15);
            }
        }
    }
}
