using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class AncientClasper : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";

        private int dust = 3;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            var modPlayer = player.CIMod();
            if (dust > 0)
            {
                int dCounts = 36;
                for (int i = 0; i < dCounts; i++)
                {
                    Vector2 dVel = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                    dVel = dVel.RotatedBy((double)((i - (dCounts / 2 - 1)) * MathHelper.TwoPi / dCounts), default) + Projectile.Center;
                    Vector2 offset = dVel - Projectile.Center;
                    int d = Dust.NewDust(dVel + offset, 0, 0, DustID.IceRod, offset.X * 1.75f, offset.Y * 1.75f, 100, default, 1.1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity = offset;
                }
                dust--;
            }
            Projectile.FramesChanger(6,5);
            Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0.05f, 0.15f, 0.2f);
            bool isMinion = Projectile.type == ModContent.ProjectileType<AncientClasper>();
            player.AddBuff(ModContent.BuffType<AncientClasperBuff>(), 3600);
            if (isMinion)
            {
                if (player.dead)
                {
                    modPlayer.IsAncientClasper = false;
                }
                if (modPlayer.IsAncientClasper)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Projectile.ChargingMinionAI(1200f, 1500f, 2200f, 150f, 0, 12f, 9f, 4f, new Vector2(0f, -60f), 12f, 18f, true, true, 2);

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            int size = tex.Height / Main.projFrames[Projectile.type];
            int y6 = size * Projectile.frame;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y6, tex.Width, size)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(tex.Width / 2f, size / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.immune[Projectile.owner] = 7;
    }
}
