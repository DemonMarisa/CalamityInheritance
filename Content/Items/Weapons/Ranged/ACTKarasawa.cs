using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ACTKarasawa : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<Karasawa>();
        }
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
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<AlgtPink>() : ModContent.RarityType<DeepBlue>();
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<ACTKarasawaHoldout>();
            Item.shootSpeed = 1f;
            Item.useAmmo = AmmoID.None;
            Item.channel = true;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 46;
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ACTKarasawaHoldout>()] < 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 targetPosition = Main.MouseWorld;
            player.itemRotation = CIFunction.CalculateItemRotation(player, targetPosition, -18);
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
