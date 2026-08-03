using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Ranged;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Ranged
{
    public class MatterModulator : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 100;
            Item.knockBack = 11f;
            Item.useTime = Item.useAnimation = 33;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.PlasmaBolt;
            Item.noMelee = true;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<UnstableMatter>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(3, 5 + 1); i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.8f, 1.3f), type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofFright, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofFright, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
