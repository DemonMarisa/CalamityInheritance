using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Summon.Normal.CloseRange;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon.Normal.CloseRange
{
    public class YharonSonStaff : CISummon
    {
        public static readonly int WeaponDamage = 160;
        public override void SetStaticDefaults()
        {
            //改为法杖的形式会更加符合这个武器的设计
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 48;
            Item.width = 56;
            Item.mana = 50;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.DamageType = DamageClass.Summon;
            Item.damage = WeaponDamage;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.FlareSound;
            Item.rare = RarityType<YharonFire>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ProjectileType<SonYharon>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
            {
                Main.projectile[p].damage = damage;
                Main.projectile[p].originalDamage = WeaponDamage;
            }
            return false;
        }
    }
}