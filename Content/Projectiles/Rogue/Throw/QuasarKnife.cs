using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Dusts;
using CalamityInheritance.Content.Items.Weapons.Rogue.Throws;
using CalamityInheritance.Content.Projectiles.Rogue.Explosion;
using CalamityInheritance.Core.Utils;
using LAP.Core.MiscDate;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Throw
{
    public class QuasarKnife : CIRogueProj
    {
        public override string Texture => GetInstance<Quasar>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
            Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.PiOver4;
            }
            float num472 = Projectile.Center.X;
            float num473 = Projectile.Center.Y;
            float num474 = 600f;
            for (int num475 = 0; num475 < Main.maxNPCs; num475++)
            {
                if (Main.npc[num475].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[num475].Center, 1, 1) && !LAPInfo.AnyBossHere)
                {
                    float num476 = Main.npc[num475].position.X + Main.npc[num475].width / 2;
                    float num477 = Main.npc[num475].position.Y + Main.npc[num475].height / 2;
                    float num478 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num476) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num477);
                    if (num478 < num474)
                    {
                        if (Main.npc[num475].position.X < num472)
                        {
                            Main.npc[num475].velocity.X += 0.25f;
                        }
                        else
                        {
                            Main.npc[num475].velocity.X -= 0.25f;
                        }
                        if (Main.npc[num475].position.Y < num473)
                        {
                            Main.npc[num475].velocity.Y += 0.25f;
                        }
                        else
                        {
                            Main.npc[num475].velocity.Y -= 0.25f;
                        }
                    }
                }
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 25f)
            {
                int numProj = 2;
                float rotation = MathHelper.ToRadians(50);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < numProj + 1; i++)
                    {
                        Vector2 speed = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
                        while (speed.X == 0f && speed.Y == 0f)
                        {
                            speed = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
                        }
                        speed.Normalize();
                        speed *= Main.rand.Next(30, 61) * 0.1f * 2.5f;
                        int knife = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, ProjectileType<Quasar2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                        if (Projectile.CI().Stealth)
                        {
                            Main.projectile[knife].SetStealthAttack();
                            Main.projectile[knife].timeLeft = 120;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileType<RadiantExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Projectile.active = false;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIAstralInfection>(), 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIAstralInfection>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<CIAstralBlue>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
