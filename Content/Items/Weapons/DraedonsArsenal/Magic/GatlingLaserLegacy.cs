using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Draedons;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Magic
{
    public class GatlingLaserLegacy : CIMagic
    {

        // This is the amount of charge consumed every time the holdout projectile fires a laser.
        public const float HoldoutChargeUse = 0.0075f;

        public override void SetDefaults()
        {
            Item.width = 43;
            Item.height = 24;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 43;
            Item.knockBack = 1f;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.channel = true;
            Item.mana = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ProjectileType<GatlingLaserHeldProj>();
            Item.shootSpeed = 24f;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddIngredient(CalamityMaterials.InfectedArmorPlating, 10).
                    AddIngredient(CalamityMaterials.LifeAlloy, 5).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }
    }
}
