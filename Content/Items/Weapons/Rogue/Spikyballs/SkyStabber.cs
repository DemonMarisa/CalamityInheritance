using CalamityInheritance.Content.Projectiles.Rogue.Spikyballs;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spikyballs
{
    public class SkyStabber : CIRogueClass
    {
        public static int knockBack = 2;
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.damage = 50;
            Item.DamageType = RogueDamageClass.Instance;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = knockBack;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 2.2f;
            Item.shoot = ModContent.ProjectileType<SkyStabberProj>();
        }

        public override float StealthDamageMultiplier => 0.5f;

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
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SkyStabberProj>(), damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            foreach(Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.owner == player.whoAmI && proj.type == ProjectileType<SkyStabberProj>())
                    proj.Kill();
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(4).
                AddIngredient(ItemID.SunplateBlock, 8).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}
