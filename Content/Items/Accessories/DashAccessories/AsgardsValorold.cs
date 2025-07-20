﻿using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer.Dash;
using CalamityMod.Buffs.DamageOverTime;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class AsgardsValorold : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public const int ShieldSlamDamage = 200;
        public const float ShieldSlamKnockback = 9f;
        public const int ShieldSlamIFrames = 12;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 44;
            Item.rare = ItemRarityID.Lime;
            Item.value = CIShopValue.RarityPriceLime;
            Item.defense = 16;
            Item.accessory = true;
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.CIMod().ValorOn;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.CIDashID = AsgardsValorDashold.ID;
            usPlayer.ValorLegacyOn = true;
            player.Calamity().DashID = string.Empty;
            player.dashType = 0;
            player.noKnockback = true;
            player.fireWalk = true;
            usPlayer.AsgardsValorImmnue = true;
            player.statLifeMax2 += 20;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true; //出于不知名原因，阿斯加德英勇不免疫地狱之火
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true; //以及硫磺火

            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            { 
                player.endurance += 0.25f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AnkhShield).
                AddIngredient<OrnateShield>().
                AddIngredient<ShieldoftheOceanLegacy>().
                AddIngredient<Abaddon>().
                AddIngredient<CoreofCalamity>().
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.AnkhShield).
                AddIngredient<OrnateShield>().
                AddIngredient<ShieldoftheOcean>().
                AddIngredient<Abaddon>().
                AddIngredient<CoreofCalamity>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
