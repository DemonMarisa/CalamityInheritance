using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using Terraria.WorldBuilding;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class CIExocometMagic : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 2;
        }

        public bool isAlive = true;
        public float currentNPCDist;
        public int foreachCD;
        public NPC target = null;
        public override void AI()
        {
            //获取玩家
            Player player = Main.player[Projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();


            Projectile.alpha += (int)Utils.GetLerpValue(0, 255, 15);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 4)
                Projectile.frame = 0;

            if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                Projectile.alpha = 128;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            Projectile.ai[1]++;

            if (Projectile.alpha < 40)
            {
                if(Projectile.ai[1] % 2 == 0)
                {
                    int exo = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
                    Main.dust[exo].velocity *= -0.25f;
                    exo = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
                    Main.dust[exo].velocity *= -0.25f;
                    Main.dust[exo].position -= Projectile.velocity * 0.5f;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.5f);

            if (Projectile.timeLeft > 280)
            {
                Projectile.velocity *= 0.97f;
            }
            // 自爆的范围
            float npcDistCompare = 800f;
            if (Projectile.timeLeft < 280)
            {
                if(Projectile.timeLeft == 279)
                {
                    target = CIFunction.FindClosestTarget(Projectile, 3000f, true, true, false);
                    if (target != null)
                        currentNPCDist = Vector2.Distance(target.Center, Projectile.Center);
                }
                foreachCD++;
                if(foreachCD % 60 == 0)
                {
                    target = CIFunction.FindClosestTarget(Projectile, 3000f, true, true, false);
                    if (target != null)
                        currentNPCDist = Vector2.Distance(target.Center, Projectile.Center);
                }
                if (foreachCD % 5 == 0)
                {
                    if (currentNPCDist < npcDistCompare)
                        isAlive = false;
                }
                if (isAlive == false && Projectile.ai[2] == 0f)
                {
                    Projectile.timeLeft = 200;
                    Projectile.ai[2] += 1f;
                    Projectile.timeLeft -= Main.rand.Next(80, 180);
                    Projectile.velocity *= 0.98f;
                }

                float maxSpeed = 18f;
                float acceleration = 0.015f * 15f;
                float homeInSpeed = MathHelper.Clamp(Projectile.ai[0] += acceleration, 0f, maxSpeed);
                if (target != null)
                    CIFunction.HomingNPCBetter(Projectile, target, 3000f, homeInSpeed, 0f, 1, null, 0.08f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();

            int heal = (int)Math.Round(hit.Damage * 0.01);
            if (heal > 100)
                heal = 100;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ModContent.ProjectileType<Exoheal>(), 3000f, 0.05f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

            int heal = (int)Math.Round(info.Damage * 0.01);
            if (heal > 100)
                heal = 100;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ModContent.ProjectileType<Exoheal>(), 3000f, 0.05f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 255, 255, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Zombie103, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 80;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
            }
            for (int j = 0; j < 10; j++)
            {
                int exoDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 0, new Color(0, 255, 255), 2.5f);
                Main.dust[exoDust].noGravity = true;
                Main.dust[exoDust].velocity *= 3f;
                exoDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                Main.dust[exoDust].velocity *= 2f;
                Main.dust[exoDust].noGravity = true;
            }
        }
    }
}
