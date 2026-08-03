using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.CraftingStations;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Ranged;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Ranged
{
    public class PulseRifleOld : CIRanged
    {
        private readonly int BaseDamage = 1200;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = BaseDamage;
            Item.knockBack = 0f;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.PulseRifleFire;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();

            Item.shoot = ProjectileType<PulseRifleShotOld>();
            Item.shootSpeed = 5f;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (velocity.Length() > 5f)
            {
                velocity.Normalize();
                velocity *= 5f;
            }

            float SpeedX = velocity.X + Main.rand.Next(-1, 2) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-1, 2) * 0.05f;

            Projectile.NewProjectile(source, position, new Vector2(SpeedX, SpeedY), ProjectileType<PulseRifleShotOld>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-10, -1);
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddIngredient(CalamityMaterials.AuricBar, 8).
                    AddIngredient(ItemID.LunarBar, 4).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();

                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddIngredient<AuricBarold>(2).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddIngredient<AuricBarold>(2).
                    AddTile<DraedonsForgeoldTile>().
                    Register();
            }
        }
    }
}
