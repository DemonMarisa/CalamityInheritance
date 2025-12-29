using CalamityInheritance.Rarity;
using CalamityInheritance.Texture;
using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class TheOldReaperLegacy : CIRogueClass
    {
        public override void ExSD()
        {
            Item.width = 106;
            Item.height = 104;
            Item.damage = 180;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ReaperProjectile>();
            Item.shootSpeed = 16f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
                return true;
            int spread = 15;
            for (int i = 0; i < 3; i++)
            {
                Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-2, 3), velocity.Y + Main.rand.Next(-2, 3)).RotatedBy(MathHelper.ToRadians(spread));
                int proj = Projectile.NewProjectile(source, position, perturbedspeed, type, (int)(damage * 0.45), knockback, player.whoAmI);
                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].Calamity().stealthStrike = true;
                }
                spread -= Main.rand.Next(5, 8);
            }
            return false;
        }
    }
}
