using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Materials;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class Sandslasher : CIRogue
    {
        public override void ExSD()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 115;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileType<SandslasherProj>();
            Item.shootSpeed = 8f;
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<SandslasherProj>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int ss = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[ss].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.GrandScale).
                    AddIngredient<CoreofSunlight>(6).
                    AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                    AddIngredient(ItemID.HardenedSand, 25).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.AncientBattleArmorMaterial, 2).
                    AddIngredient<CoreofSunlight>(6).
                    AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                    AddIngredient(ItemID.HardenedSand, 25).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
