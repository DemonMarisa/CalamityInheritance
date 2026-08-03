using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Wulfrum;
using CalamityInheritance.Core.Utils;
using CalamityMod.Projectiles.Rogue;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Wulfrum
{
    public class WulfrumKnifeOld : CIRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 22;
            Item.damage = 11;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 38;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ProjectileType<WulfrumKnifeProjOld>();
            Item.shootSpeed = 12f;
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<WulfrumKnifeProjOld>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CISoundID.SoundWeaponSwing, player.Center);
            int p = Projectile.NewProjectile(source, position, velocity * 1.3f, ProjectileType<WulfrumKnifeProjOld>(), damage, knockback, player.whoAmI);
            Projectile proj = Main.projectile[p];
            proj.SetStealthAttack();
            proj.penetrate = 4;
            proj.usesLocalNPCImmunity = true;
            proj.localNPCHitCooldown = 1;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.WulfrumMetalScrap, 12).
                    AddTile(TileID.Anvils).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnySilverBar, 12).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}
