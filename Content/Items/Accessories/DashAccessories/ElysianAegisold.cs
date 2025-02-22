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
    public class ElysianAegisold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public const int ShieldSlamDamage = 500;
        public const float ShieldSlamKnockback = 15f;
        public const int ShieldSlamIFrames = 12;

        public const int RamExplosionDamage = 500;
        public const float RamExplosionKnockback = 20f;

        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityInheritanceKeybinds.AegisHotKey);
        public override void SetStaticDefaults()
        {

            // if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            // {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ElysianAegis>()] = ModContent.ItemType<ElysianAegisold>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ElysianAegisold>()] = ModContent.ItemType<ElysianAegis>();
            // }
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 42;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
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
            player.Calamity().DashID = string.Empty;
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
        public override void AddRecipes()
        {
            CreateRecipe().
                    AddIngredient<LoreProvidence>().
                    AddTile(TileID.LunarCraftingStation).
                    Register();

            CreateRecipe().
                    AddIngredient<KnowledgeProvidence>().
                    AddTile(TileID.LunarCraftingStation).
                    Register();
        }
    }
}
