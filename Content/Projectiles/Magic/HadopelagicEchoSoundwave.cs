using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class HadopelagicEchoSoundwave : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Magic/EidolicWailSoundwave";

        private int echoCooldown = 0;
        private bool playedSound = false;
        private static int penetrationAmt = 50;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.scale = 0.005f;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = penetrationAmt;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.timeLeft = 450;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f || Projectile.ai[0] == 4f)
            {
                if (!playedSound)
                {
                    SoundEngine.PlaySound(HadopelagicEcho.UseSound, Projectile.Center);
                    playedSound = true;
                }
            }

            if (Projectile.localAI[0] < 1f)
            {
                Projectile.localAI[0] += 0.05f;
                Projectile.scale += 0.05f;
                Projectile.width = (int)(36f * Projectile.scale);
                Projectile.height = (int)(36f * Projectile.scale);
            }
            else
            {
                Projectile.width = 36;
                Projectile.height = 36;
            }
            if (echoCooldown > 0)
                echoCooldown--;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;

            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.damage = (int)((double)Projectile.damage * Projectile.localAI[0] * (0.5D + (0.5D / Projectile.penetrate)));
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
            target.AddBuff(BuffID.Ichor, 300);
            target.AddBuff(BuffID.Electrified, 600);
            Projectile.velocity *= 0.85f;

            if (echoCooldown <= 0)
            {
                echoCooldown = 60;
                int echoID = ModContent.ProjectileType<HadopelagicEcho2>();
                int echoDamage = (int)(0.2f * Projectile.damage);
                float echoKB = Projectile.knockBack / 3;
                int echos = 5;
                for (int i = 0; i < echos; ++i)
                {
                    float startDist = Main.rand.NextFloat(260f, 270f);
                    Vector2 startDir = Main.rand.NextVector2Unit();
                    Vector2 startPoint = target.Center + (startDir * startDist);

                    float echoSpeed = Main.rand.NextFloat(15f, 18f);
                    Vector2 velocity = startDir * (-echoSpeed);

                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),startPoint, velocity, echoID, echoDamage, echoKB, Projectile.owner);
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * ((float)b2 / 255f));
                return new Color((int)b2, (int)b2, (int)b2, (int)a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
