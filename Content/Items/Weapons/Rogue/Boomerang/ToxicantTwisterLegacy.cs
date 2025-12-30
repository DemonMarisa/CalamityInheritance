using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityInheritance.Rarity;
using CalamityInheritance.Texture;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class ToxicantTwisterLegacy : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Rogue";
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 46;
            Item.damage = 333;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ToxicantTwisterProjLegacy>();
            Item.shootSpeed = 18f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override float StealthDamageMultiplier => 1.3f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int boomer = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (boomer.WithinBounds(Main.maxProjectiles))
                    Main.projectile[boomer].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
