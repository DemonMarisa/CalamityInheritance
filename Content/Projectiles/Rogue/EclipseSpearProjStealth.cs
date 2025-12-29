using System.Collections.Generic;
using System.IO;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Particles;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseSpearProjStealth : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponPath}/Rogue/EclipseSpear";
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
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.extraUpdates = 4;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 150 * Projectile.extraUpdates;
        }
        public bool isSticky = false;
        public bool ResetProj = false;
        public int Target
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public int timer = 0;
        public int SpawnCount = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(isSticky);
            writer.Write(ResetProj);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            isSticky = reader.ReadBoolean();
            ResetProj = reader.ReadBoolean();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.3f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if (Main.rand.NextBool(2))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                Particle eclipseTrai2 = new StarProjBlack(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                GeneralParticleHandler.SpawnParticle(Main.rand.NextBool() ? eclipseTrail : eclipseTrai2);
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
            CIFunction.HomeInOnNPC(Projectile, !Projectile.tileCollide, 2500f, 18f, 0, 0.3f);
        }
        public void StickingAI()
        {
            if (!ResetProj)
            {
                Projectile.extraUpdates = 1;
                Projectile.localNPCHitCooldown = 60;
                Projectile.timeLeft = 600;
                ResetProj = true;
            }
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
        public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            if (!isSticky)
            {
                Projectile.ai[0] = npc.whoAmI;
                Projectile.velocity = (npc.Center - Projectile.Center) * 0.75f;
                isSticky = true;
            }
            else
            {
                OnHitSparks();
                //击中一次，产生一次爆炸
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
            }
            SoundEngine.PlaySound(CISoundMenu.EclipseSpearBoom, npc.Center);
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
                Particle eclipseTrail = new SparkParticle(Projectile.Center, sVel, false, sLife, sScale, trailColor);
                Particle eclipseTrai2 = new StarProjBlack(Projectile.Center, sVel, false, sLife, sScale, Color.Black);
                GeneralParticleHandler.SpawnParticle(Main.rand.NextBool() ? eclipseTrail : eclipseTrai2);
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (isSticky) 
            {
				int npcIndex = (int)Projectile.ai[0];
				if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active) {
					if (Main.npc[npcIndex].behindTiles) {
						behindNPCsAndTiles.Add(index);
					}
					else {
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
            int pAmt = Main.rand.Next(2,3);
            for (int i = 0; i < pAmt; i++)
            {
                //随机水平位置
                float pSummonPosX = tarPos.X + Main.rand.NextFloat(-200f, 201f);
                //生成的高度
                float pSummonPosY = tarPos.Y - Main.rand.NextFloat(550f, 880f);
                Vector2 pPos = new (pSummonPosX, pSummonPosY);
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
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.ai[0] == 1f ? false : base.CanHitNPC(target);

        public override bool CanHitPvp(Player target) => Projectile.ai[0] != 1f;
    }
}