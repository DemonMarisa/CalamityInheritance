using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ProfanedLancher: CIRanged, ILocalizedModType
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
            Item.rare = CIConfig.Instance.SpecialRarityColor? RarityType<IchikaBlack>() : RarityType<BlueGreen>();
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
            if (Main.zenithWorld)
            {
                for (int i = 0; i < 12 ; i++)
                {
                    Vector2 spreading = new Vector2(velocity.X, 0).RotatedByRandom(180f);
                    Projectile.NewProjectile(source, position, spreading, ProjectileType<ProfanedNuke>(), damage, knockback);
                }
                return false;
            }
            else
                Projectile.NewProjectile(source, position, velocity, ProjectileType<ProfanedNuke>(), damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UnholyEssence>(12).
                AddIngredient<DivineGeode>(6).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}