using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.CIPlayer.Dash;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class ElysianAegisold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public const int ShieldSlamDamage = 500;
        public const float ShieldSlamKnockback = 15f;
        public const int ShieldSlamIFrames = 12;

        public const int RamExplosionDamage = 500;
        public const float RamExplosionKnockback = 20f;

        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityInheritanceKeybinds.AegisHotKey);
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 42;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.defense = 18;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();

            // Elysian Aegis ram dash
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            modPlayer1.CIDashID = ElysianAegisDashold.ID;
            modPlayer1.elysianAegis = true;

            player.dashType = 0;

            // Vaguely inherited Ankh Shield effects I guess
            player.noKnockback = true;
            player.fireWalk = true;

            // Debuff immunities
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.ShadowFlame] = true;
            player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            player.buffImmune[BuffID.Daybreak] = true;
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            player.statLifeMax2 += 40;
            player.lifeRegen += 4;
        }
    }
}
