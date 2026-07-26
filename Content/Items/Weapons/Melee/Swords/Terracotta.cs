using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Content.Projectiles.Melee.Explosions;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class Terracotta : CIMelee
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
                player.SpawnHealProj(target.GetSource_FromThis(), ProjectileType<PhoenixBladeHeal>(), target.Center, new Vector2(randomSpeedX, randomSpeedY));
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GoldCoin);
        }
    }
}
