using System;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuUltraBlade : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => GenericProjRoute.InvisProjRoute;
        internal SoundStyle[] HitSounds =
        [
            PBGLegendaryBeam.HitSound1,
            PBGLegendaryBeam.HitSound2,
            PBGLegendaryBeam.HitSound3
        ];
        internal static Color ShizukuAqua => new(152, 245, 255);
        internal static Color ShizukuSilver => new(248, 248, 255);
        internal Color[] ShizukuColor =
        [
            ShizukuAqua,
            ShizukuSilver
        ];
        public Player Owner => Main.player[Projectile.owner];
        public ref float AttackType => ref Projectile.ai[1];
        public ref float AttackTimer => ref Projectile.ai[2];
        public int TargetIndex
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        const float IsSpawned = 0f;
        const float IsHit = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 12;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            Color useColor = Utils.SelectRandom(Main.rand, ShizukuColor);
            if (Main.rand.NextBool(3))
            {
                FlyingDust();
            }
            SparkParticle line = new SparkParticle(Projectile.Center, Projectile.velocity * 0.01f, false, 18, 1.2f, useColor);
            GeneralParticleHandler.SpawnParticle(line);
            ThisAttackAI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle hitSound = Utils.SelectRandom(Main.rand, HitSounds);
            SoundEngine.PlaySound(hitSound with { MaxInstances = 0 }, target.Center);
            //确认是否为正确的敌怪单位
            NPC npc = Main.npc[TargetIndex];
            if (npc is null)
                return;
            
            if (npc.whoAmI == target.whoAmI)
            {
                AttackType = IsHit;
                Projectile.netUpdate = true;

            }
        }
        private void FlyingDust()
        {
            int dType = DustID.GemDiamond;
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dType, 0f, 0f, 100, Color.White, 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0f;
                Main.dust[d].color = Utils.SelectRandom(Main.rand,ShizukuColor);
            }
        }

        private void ThisAttackAI()
        {
            NPC target = Main.npc[TargetIndex];
            if (target is null)
            {
                Projectile.Kill();
                Projectile.netUpdate = true;
                return;
            }
            //距离无所谓，反正这个属性无视
            if (AttackType == IsSpawned)
                Projectile.HomingNPCBetter(target, 1f, 24f, 20f, 2, ignoreDist: true);
            else
            {
                AttackTimer++;
                if (AttackTimer > 10f * Projectile.extraUpdates)
                {
                    Projectile.Kill();
                }
            }
        }
        
    }
}