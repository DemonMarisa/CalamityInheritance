using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.GreatStaff;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.GreatStaff
{
    public class VehemencOld : CIMagic
    {

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.75f;
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<VehemenceOld>();
            Item.shootSpeed = 16f;

            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<VehemenceOld>(), damage, (int)knockback, player.whoAmI, 0f, 0f);
            player.AddBuff(BuffID.ManaSickness, 600, true);
            return false;
        }
    }
}
