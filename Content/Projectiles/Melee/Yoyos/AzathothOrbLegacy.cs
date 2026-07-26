using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using CalamityMod;
using LAP.Core.Graphics.DeepGlow;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Yoyos
{
    public class AzathothOrbLegacy : CIMeleeProj
    {
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5; //10->5
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.075f, 0.5f, 0.15f));

            Projectile.velocity *= 0.985f;
            Projectile.rotation += Projectile.velocity.X * 0.2f;

            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += 0.08f;
            }
            else
            {
                Projectile.rotation -= 0.08f;
            }

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 30f)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha >= 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                    return;
                }
            }
            Projectile.localAI[2]++;
            if (Projectile.localAI[2] % 10f == 0)
            {
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center,400);
                if (npc is not null)
                {
                    Vector2 ToNPC = LAPUtilities.GetVector2(Projectile.Center, npc.Center);
                    Projectile.NewProj(ProjectileType<AzathothBoltLegacy>(), Projectile.Center, ToNPC * 8);
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dustScale = (int)(10f * Projectile.scale);
                int d = Dust.NewDust(Projectile.Center - Vector2.One * dustScale, dustScale * 2, dustScale * 2, DustID.PinkTorch, 0f, 0f, 0, default, 1f);
                Dust dust = Main.dust[d];
                Vector2 offset = Vector2.Normalize(dust.position - Projectile.Center);
                dust.position = Projectile.Center + offset * dustScale * Projectile.scale;
                if (i < 30)
                {
                    dust.velocity = offset * dust.velocity.Length();
                }
                else
                {
                    dust.velocity = offset * Main.rand.NextFloat(4.5f, 9f);
                }
                dust.color = Main.hslToRgb(0.95f, 0.41f + Main.rand.NextFloat() * 0.2f, 0.93f);
                dust.color = Color.Lerp(dust.color, Color.White, 0.3f);
                dust.noGravity = true;
                dust.scale = 0.7f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(lightColor.R - Projectile.alpha, lightColor.G - Projectile.alpha, lightColor.B - Projectile.alpha, lightColor.A - Projectile.alpha);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color drawColor = Color.HotPink;
            Projectile.GetProjDrawInfo_Normal(out Texture2D texture, out Vector2 drawPos, out float drawRot, out Vector2 orig, out SpriteEffects spriteEffects);
            float angle = MathHelper.TwoPi / 10;
            for (int i = 0; i < 10; i++)
            {
                Vector2 offset = new Vector2(2.5f * Main.rand.NextFloat(0.8f, 1.3f), 0).RotatedBy(angle * i);
                LAPUtilities.Draw(texture, drawPos + offset, null, drawColor with { A = 0}, drawRot, orig, Projectile.scale, 0);
            }
            LAPUtilities.Draw(texture, drawPos, null, Projectile.GetAlpha(drawColor), drawRot, orig, Projectile.scale, 0);
            return false;
        }
    }
}
