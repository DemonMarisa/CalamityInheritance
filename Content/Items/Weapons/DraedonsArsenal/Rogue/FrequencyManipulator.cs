using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Rogue;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Rogue
{
    public class FrequencyManipulator : CIRogue
    {
        public override void ExSD()
        {
            Item.width = 26;
            Item.height = 44;
            Item.damage = 80;
            Item.DamageType = RogueDamage.Instance;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 56;
            Item.useAnimation = 56;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;

            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<FrequencyManipulatorProjectile>();

            Item.LAP().SkillShoot = ProjectileType<FrequencyManipulatorProjectile>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[proj].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 8).
                    AddIngredient(CalamityMaterials.DubiousPlating, 12).
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofSight, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofSight, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
