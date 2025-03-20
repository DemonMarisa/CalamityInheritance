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
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ElysianAegis>()] = ModContent.ItemType<ElysianAegisold>();
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ElysianAegisold>()] = ModContent.ItemType<ElysianAegis>();
            Item.ResearchUnlockCount = 1;
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
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.CIDashID = ElysianAegisDashold.ID;
            usPlayer.ElysianAegis = true;
            usPlayer.ElysianAegisImmnue = true;
            
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;

            player.Calamity().DashID = string.Empty;
            player.dashType = 0;

            player.noKnockback = true;
            player.fireWalk = true;
            
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
