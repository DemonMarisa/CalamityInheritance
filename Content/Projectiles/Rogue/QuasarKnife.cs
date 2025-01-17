using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class QuasarKnife : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Quasar");
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
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 2.355f;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= 1.57f;
            }
            float num472 = Projectile.Center.X;
            float num473 = Projectile.Center.Y;
            float num474 = 600f;
            for (int num475 = 0; num475 < Main.maxNPCs; num475++)
            {
                if (Main.npc[num475].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[num475].Center, 1, 1) && !CalamityPlayer.areThereAnyDamnBosses)
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
                        int knife = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<Quasar2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                        Main.projectile[knife].Calamity().stealthStrike = Projectile.Calamity().stealthStrike;
                        if (Projectile.Calamity().stealthStrike)
                        {
                            Main.projectile[knife].timeLeft = 120;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RadiantExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Projectile.active = false;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
