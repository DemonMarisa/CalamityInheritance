using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.CAWeapons.Helds;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.CAWeapons.Magic
{
    public class ACTAlphaRay : CIMagic
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 74;
            Item.damage = 180;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.UseSound = CISoundID.SoundLaserDestroyer;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ProjectileType<ACTGenisisHeldProj>();
            Item.rare = RarityType<AlgtPink>();

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<ACTGenisisHeldProj>()] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ProjectileType<ACTGenisisHeldProj>()] < 1)
                Projectile.NewProjectile(source, position, velocity, ProjectileType<ACTGenisisHeldProj>(), damage, knockback, player.whoAmI, 1f, 0f, 0f);

            if (player.ownedProjectileCounts[ProjectileType<AlphaWingmanHeldProj>()] < 1)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<AlphaWingmanHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
        }
    }
}
