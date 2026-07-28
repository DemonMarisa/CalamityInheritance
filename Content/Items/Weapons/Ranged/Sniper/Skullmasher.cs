using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Content.Projectiles.Ranged.Sniper;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Sniper
{
    public class Skullmasher : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 1020;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.crit += 5;
            Item.height = 30;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useAmmo = 97;
            Item.rare = RarityType<MaliceChallengeDrop>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-40, 0);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMarkedforDeath>(), 300);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CISounds.LargeWeaponFire);
            for (int projectiles = 0; projectiles < 3; projectiles++)
            {
                float speedX = velocity.X + Main.rand.Next(-40, 41) * 0.01f;
                float speedY = velocity.Y + Main.rand.Next(-40, 41) * 0.01f;
                if (type == ProjectileID.Bullet)
                    type = ProjectileType<BetterAMR>();
                Projectile proj = Projectile.NewProjectileDirect(source, position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);
                if (type != ProjectileType<BetterAMR>())
                    proj.CI().AMRextra = true;
                else if (type == ProjectileType<BetterAMR>())
                    proj.ai[0] = 4;
            }
            return false;
        }
        public override void AddRecipes()
        {
        }
    }
}
