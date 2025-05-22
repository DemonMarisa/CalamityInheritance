using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic.Alpha;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class ACTAlphaRay : CIMagic, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<AlphaRayLegacy>(false);
        }


        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 74;
            Item.damage = 180;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.UseSound = CISoundID.SoundLaserDestroyer;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<ACTAlphaHeldProj>();
            Item.rare = CIConfig.Instance.SpecialRarityColor? ModContent.RarityType<AlgtPink>() : ModContent.RarityType<DeepBlue>();

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<ACTAlphaHeldProj>()] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ACTAlphaHeldProj>()] < 1)
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ACTAlphaHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);

            if (player.ownedProjectileCounts[ModContent.ProjectileType<AlphaWingmanHeldProj>()] < 1)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AlphaWingmanHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ACTGenisis>().
                AddIngredient<ACTWingman>(2).
                AddIngredient<GalacticaSingularity>(5).
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
