using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityInheritance.Rarity;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ACTMinigun : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        
        public override void SetDefaults()
        {
            Item.damage = 285;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 92;
            Item.height = 44;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.UseSound = CISoundID.SoundChainGun;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<KingsbaneHoldoutReal>();
            Item.shootSpeed = 22f;
            Item.useAmmo = AmmoID.Bullet;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<AlgtPink>():ModContent.RarityType<CatalystViolet>();

            Item.channel = true;
            Item.noUseGraphic = true;
            Item.UseSound = null;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile holdout = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<KingsbaneHoldoutReal>(), damage, knockback, player.whoAmI, 0f, 0f);
            holdout.velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);

            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.8f;
    }
}
