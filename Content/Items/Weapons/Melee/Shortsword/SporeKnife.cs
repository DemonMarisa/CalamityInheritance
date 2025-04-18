﻿using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class SporeKnife : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.33f;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 28;
            Item.height = 28;
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SporeKnifeProj>();
            Item.shootSpeed = 2.4f;
            Item.rare = ItemRarityID.Green;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.JungleSpores, 10).
                AddIngredient(ItemID.Stinger, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
