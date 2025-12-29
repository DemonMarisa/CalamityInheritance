using Terraria.DataStructures;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class SpearofDestinyLegacy :CIRogueClass
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 52;
            Item.damage = 42;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 2f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 52;
            Item.rare = RarityType<MaliceChallengeDrop>();   
            Item.shoot = ProjectileType<SpearofDestinyProjLegacy>();
            Item.shootSpeed = 20f;
            Item.value = CIShopValue.RarityMaliceDrop;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = 7;
            if (player.Calamity().StealthStrikeAvailable())
            {
                for (int i = -2 * index; i <= 2 * index; i += index)
                {
                    int projType = type;
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                    int spear = Projectile.NewProjectile(source, position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                    if (spear.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[spear].Calamity().stealthStrike = true;
                        Main.projectile[spear].extraUpdates += 2;
                    }
                }
                return false;
            }
            for (int i = -index; i <= index; i += index)
            {
                int projType = (i != 0 || player.Calamity().StealthStrikeAvailable()) ? type : ProjectileType<IchorSpearProjLegacy>();
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                int spear = Projectile.NewProjectile(source, position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                if (spear.WithinBounds(Main.maxProjectiles))
                    Main.projectile[spear].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            }
            return false;
        }
    }
}
