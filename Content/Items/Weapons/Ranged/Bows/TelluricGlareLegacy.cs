using CalamityInheritance.Content.Projectiles.Ranged.Bows;
using CalamityInheritance.Rarity;
using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using LAP.Core.Enums;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bows
{
    public class TelluricGlareLegacy : CIRanged, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 92;
            Item.damage = 216;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.reuseDelay = 23;
            Item.useLimitPerAnimation = 4;
            Item.knockBack = 7.5f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();

            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<TelluricGlareArrowLegacy>();
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;

            Item.LAP().UseCICalStatInflation = true;
            Item.LAP().WeaponTier = AllWeaponTier.PostProvidence;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-14f, 0f);

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Always fires Radiant Arrows regardless of ammo used
            type = Item.shoot;
            Vector2 offset = Vector2.Normalize(velocity.RotatedBy(MathHelper.PiOver2));
            position += offset * Main.rand.NextFloat(-19f, 19f);
            position -= 3f * velocity;
        }
    }
}
