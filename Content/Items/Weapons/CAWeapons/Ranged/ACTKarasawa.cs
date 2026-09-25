using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.CAWeapons.Helds;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.CAWeapons.Ranged
{
    public class ACTKarasawa : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 44;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 14687;
            Item.knockBack = 12f;
            Item.useTime = 52;
            Item.useAnimation = 52;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<AlgtPink>();

            Item.shoot = ProjectileType<ACTKarasawaHoldout>();
            Item.shootSpeed = 1f;
            Item.useAmmo = AmmoID.None;
            Item.channel = true;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 46;
        public override bool CanUseItem(Player player) => !player.HasProj(Item.shoot);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile holdout = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<ACTKarasawaHoldout>(), damage, knockback, player.whoAmI, 0f, 0f);
            holdout.velocity = (player.LocalMouseWorld() - player.MountedCenter).SafeNormalize(Vector2.Zero);

            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }
    }
}
