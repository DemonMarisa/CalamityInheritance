using CalamityMod.Buffs.StatDebuffs;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class TerratomereProjectile : ModProjectile, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = Main.zenithWorld ? 0.5f : 1f;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Main.zenithWorld ? 360 : 180;
            Projectile.tileCollide = false;
            AIType = ProjectileID.TerraBeam;
        }

        public override void AI()
        {
            if (!Main.zenithWorld)
            CalamityUtils.HomeInOnNPC(Projectile, true, 400f, 12f, 20f);
            else
            CIFunction.HomeInOnNPC(Projectile, true, 1800f, 14f, 90f);

            Lighting.AddLight(Projectile.Center, 0f, (255 - Projectile.alpha) * 0.75f / 255f, 0f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 200, 0, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[base.Projectile.owner];
            target.AddBuff(ModContent.BuffType<GlacialState>(), 30);
            if (target.type != NPCID.TargetDummy && target.canGhostHeal && !player.moonLeech)
            {
                int healAmount = Main.rand.Next(1) + 1;
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            int num3;
            for (int num795 = 4; num795 < 31; num795 = num3 + 1)
            {
                float num796 = Projectile.oldVelocity.X * (30f / num795);
                float num797 = Projectile.oldVelocity.Y * (30f / num795);
                int num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[num798].noGravity = true;
                Dust dust = Main.dust[num798];
                dust.velocity *= 0.5f;
                num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                dust = Main.dust[num798];
                dust.velocity *= 0.05f;
                num3 = num795;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
