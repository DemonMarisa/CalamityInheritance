using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.CIPlayer.Dash;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Content.Items.LoreItems;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class ElysianAegisold : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public const int ShieldSlamDamage = 500;
        public const float ShieldSlamKnockback = 15f;
        public const int ShieldSlamIFrames = 12;

        public const int RamExplosionDamage = 500;
        public const float RamExplosionKnockback = 20f;

        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityInheritanceKeybinds.AegisHotKey);
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:48,
            itemHeight:42,
            itemRare: RarityType<BlueGreen>(),
            itemValue:CIShopValue.RarityPriceBlueGreen,
            itemDefense:18
        );
        public override void ExSSD()
        {
            Type.ShimmerEach<ElysianAegis>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.CIDashID = ElysianAegisDashold.ID;
            usPlayer.ElysianAegis = true;
            usPlayer.ElysianAegisImmnue = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
            player.buffImmune[BuffType<HolyFlames>()] = true;
            player.buffImmune[BuffType<BrimstoneFlames>()] = true;

            player.Calamity().DashID = string.Empty;
            player.dashType = 0;

            player.noKnockback = true;
            player.fireWalk = true;
            
            player.statLifeMax2 += 40;
            player.lifeRegen += 4;
        }
    }
}
