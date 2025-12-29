using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Typeless.Heal;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Terracotta : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 62;
            Item.scale = 1.5f;
            Item.damage = 110;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, 0f, ProjectileType<TerracottaExplosion>(), hit.Damage, hit.Knockback, player.whoAmI);
            if (target.life >= 0)
                return;

            for (int i = -1; i < 2; i++)
            {
                float randomSpeedX = Main.rand.Next(3);
                float randomSpeedY = Main.rand.Next(3, 5);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, randomSpeedX * i, -randomSpeedY, ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hit)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, 0f, ProjectileType<TerracottaExplosion>(), hit.Damage, hit.Knockback, player.whoAmI);
            if (target.statLife >= 0)
                return;

            for (int i = -1; i < 2; i++)
            {
                float randomSpeedX = Main.rand.Next(3);
                float randomSpeedY = Main.rand.Next(3, 5);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, randomSpeedX * i, -randomSpeedY, ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 246);
        }
    }
}
