using CalamityInheritance.Content.Projectiles.Rogue.Darts;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Darts
{
    public class RefractionRotorLegacy : CIRogueClass
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 120;
            Item.damage = 616;
            Item.knockBack = 8.5f;
            Item.useAnimation = Item.useTime = 18;
            Item.DamageType = RogueDamageClass.Instance;
            Item.autoReuse = true;
            Item.shootSpeed = 18f;
            Item.shoot = ProjectileType<RefractionRotorProjectileLegacy>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
        }

        public override float StealthDamageMultiplier => 0.75f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int shuriken = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && Main.projectile.IndexInRange(shuriken))
                Main.projectile[shuriken].Calamity().stealthStrike = true;
            return false;
        }
    }
}
