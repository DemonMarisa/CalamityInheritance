using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Rogue.Spear;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using LAP.Content.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class EclipseSpearProjStealth : BaseStickyProj, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.RogueProj}";
        public override string Texture => GetInstance<EclipseSpear>().Texture;
        public int timer;
        public int SpawnCount;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.extraUpdates = 4;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.timeLeft = 150 * Projectile.extraUpdates;
        }
        public override void ExAI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.3f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if (Main.rand.NextBool(2))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                if (Main.rand.NextBool())
                {
                    SparkParticle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                    eclipseTrail.Spawn();
                }
                else
                {
                    SparkParticle eclipseTrai2 = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                    eclipseTrai2.SpawnToNonPreMult();
                }
            }
            if (!isSticky)
                NormalAI();
            else
                StickingAI();
        }
        public void NormalAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.3f);
            // 不跟踪了，改为有极高限制角度的跟踪
            LAPUtilities.HomeInNPC(Projectile, 2500f, 18f, 0, 0.3f, !Projectile.tileCollide);
        }
        public void StickingAI()
        {
            if (!CIUtils.InBounds(Target, Main.npc.Length))
                return;
            // 如果目标可以受击且活动，就进行挂载，否则删除弹幕
            if (Main.npc[Target].active && !Main.npc[Target].dontTakeDamage)
            {
                Projectile.Center = Main.npc[Target].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[Target].gfxOffY;
                if (timer > 0)
                    timer--;
                if (timer <= 0)
                {
                    RainDownSpears();
                    SpawnCount++;
                    timer = 40;
                    if (SpawnCount >= 2)
                    {
                        SpawnCount = 0;
                        timer = 90;
                    }
                }
            }
            else
            {
                // 生成一个爆炸并Kill掉
                RainDownSpears();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
                Projectile.Kill();
            }
        }
        public override void ExOnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.LAP().OnceHitEffect)
            {
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
                OnHitSparks();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
            }
            SoundEngine.PlaySound(CISounds.EclipseSpearBoom, target.Center);
        }
        public void OnHitSparks()
        {
            int sparkCount = Main.rand.Next(12, 24);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.6f, 3f);
                int sLife = Main.rand.Next(30, 60);
                float sScale = Main.rand.NextFloat(1.6f, 2f) * 0.955f;
                Color trailColor = Color.DarkOrange;
                if (Main.rand.NextBool())
                {
                    SparkParticle eclipseTrail = new SparkParticle(Projectile.Center, sVel, false, sLife, sScale, trailColor);
                    eclipseTrail.Spawn();
                }
                else
                {
                    SparkParticle eclipseTrai2 = new SparkParticle(Projectile.Center, sVel, false, sLife, sScale, Color.Black);
                    eclipseTrai2.SpawnToNonPreMult();
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (isSticky)
            {
                int npcIndex = (int)Projectile.ai[0];
                if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
                {
                    if (Main.npc[npcIndex].behindTiles)
                    {
                        behindNPCsAndTiles.Add(index);
                    }
                    else
                    {
                        behindNPCsAndTiles.Add(index);
                    }
                    return;
                }
            }
            behindNPCsAndTiles.Add(index);
        }
        private void RainDownSpears()
        {
            Vector2 tarPos = Projectile.Center;
            int pAmt = Main.rand.Next(2, 3);
            for (int i = 0; i < pAmt; i++)
            {
                //随机水平位置
                float pSummonPosX = tarPos.X + Main.rand.NextFloat(-200f, 201f);
                //生成的高度
                float pSummonPosY = tarPos.Y - Main.rand.NextFloat(550f, 880f);
                Vector2 pPos = new(pSummonPosX, pSummonPosY);
                //速度
                Vector2 speed = tarPos - pPos;
                //水平速度一点随机读
                speed.X += Main.rand.NextFloat(-15f, 16f);
                float pSpeed = 24f;
                float tarDist = speed.Length();
                //固定格式
                tarDist = pSpeed / tarDist;
                speed.X *= tarDist;
                speed.Y *= tarDist;
                //生崽
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pPos, speed, ProjectileType<EclipseSpearSmall>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}