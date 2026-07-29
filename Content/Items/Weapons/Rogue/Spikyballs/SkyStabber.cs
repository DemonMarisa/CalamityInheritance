using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Spikyballs;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spikyballs
{
    public class SkyStabber : CIRogue
    {
        public static int knockBack = 2;
        public override void ExSD()
        {
            Item.width = 16;
            Item.height = 16;
            Item.damage = 50;
            Item.DamageType = RogueDamage.Instance;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = knockBack;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 2.2f;
            Item.shoot = ModContent.ProjectileType<SkyStabberProj>();

            Item.LAP().SkillShoot = ProjectileType<SkyStabberProj>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileID.None;
                return player.ownedProjectileCounts[ModContent.ProjectileType<SkyStabberProj>()] > 0;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<SkyStabberProj>();
                return player.ownedProjectileCounts[ModContent.ProjectileType<SkyStabberProj>()] < 4;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SkyStabberProj>(), damage, knockback, player.whoAmI);
            Main.projectile[stealth].SetStealthAttack();
        }
        public override bool AltFunctionUse(Player player)
        {
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.owner == player.whoAmI && proj.type == ProjectileType<SkyStabberProj>())
                    proj.Kill();
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(CalamityMaterials.AerialiteBar, 4).
                AddIngredient(ItemID.SunplateBlock, 8).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}
