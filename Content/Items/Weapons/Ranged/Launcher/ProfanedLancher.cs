using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Ranged.Launcher;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Launcher
{
    public class ProfanedLancher : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            //属性赋值的原灾的
            Item.width = 66;
            Item.height = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 270;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.autoReuse = true;
            Item.UseSound = CISoundID.SoundGrenadeLanucher;
            Item.shootSpeed = 22f;
            Item.shoot = ProjectileType<ProfanedNuke>();
            Item.useAmmo = AmmoID.Rocket;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
        public int WhatRocket;
        public override void OnConsumeAmmo(Item ammo, Player player) => WhatRocket = ammo.type;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.UnholyEssence, 12).
                    AddIngredient(CalamityMaterials.DivineGeode, 6).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}