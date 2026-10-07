using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class PwnagehammerProj_Rogue : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<Pwnagehammer>();
        public override string Texture => GetInstance<Pwnagehammer>().Texture;
        public static readonly SoundStyle AdditionHitSigSound = CISounds.PwnagehammerSound with { Volume = 0.30f };
        public NPC target;
        public int AttackType;
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = true;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 240;
            Projectile.netImportant = true;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.timeLeft = 2;
            Player owner = Main.player[Projectile.owner];
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
            if (Projectile.soundDelay == 0 && Projectile.ai[0] != 2f)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            Timer++;
            target = LAPUtilities.FindClosestTarget(Projectile.Center, 1800f);
            if (Timer == 48)
            {
                if (target is null)
                    AttackType = 2;
                else
                    AttackType = 1;
            }
            if (AttackType == 1)
            {
                if (target is not null && Timer < 2400)
                {
                    OnChasingDust();
                    Projectile.HomingTarget(target.Center, 1800, 24f, 4f);
                }
                else
                {
                    if (Timer >= 2400)
                        AttackType = 2;
                    target = LAPUtilities.FindClosestTarget(Projectile.Center, 1800f);
                    if (target is null)
                        AttackType = 2;
                }
            }
            else if (AttackType == 2)
            {
                Projectile.PredictHomeIn(12f, 1.6f, Projectile.Owner());
                if (Projectile.IsLocalPlayer() && Projectile.Hitbox.Intersects(owner.Hitbox))
                {
                    DustCircle(Projectile.Center, 21, 2f, CIDustID.DustSandnado, true, 9f);
                    SoundEngine.PlaySound(AdditionHitSigSound with { Pitch = 0.2f });
                    Projectile.Kill();
                }
            }
            //无论状态，锤子都应当在飞行过程中旋转速度增快
            Projectile.rotation += 0.23f;
            return;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //击中时造成神圣之火
            target.AddBuff(BuffType<CIHolyFlames>(), 240);
            if (AttackType == 1)
                OnStuckEffect();
            else
                OnHitDust();
        }
        private void OnStuckEffect()
        {
            DustCircle(Projectile.Center, 16f, 2.2f, CIDustID.DustSandnado, true, 9f, default, default, 6f);
            SoundEngine.PlaySound(AdditionHitSigSound with { Pitch = 0.15f }, Projectile.Center);
        }
        private void OnChasingDust()
        {
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.4f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.4f + velOffset.Y;

                //追踪时不让金色粒子拥有速度
                dFlyVelX *= 0f;
                dFlyVelY *= 0f;
                offset *= 1.15f;
                float dScale = 1.6f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, CIDustID.DustSandnado, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.5f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.5f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度更快, 粒子大小更大, 且偏移也会更大一些
                dFlyVelX *= 1.25f;
                dFlyVelY *= 1.25f;
                offset *= 1.05f;
                float dScale = 1.6f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.GemRuby, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }
        }
        private void OnHitDust() //击中时生成神圣粒子
        {
            float dCounts = 15f;
            float rotArg = 360f / 10f;
            for (int i = 0; i < dCounts; ++i)
            {
                float rotate = MathHelper.ToRadians(i * rotArg);
                Vector2 dPos = new Vector2(4.8f, 0).RotatedBy(rotate * Main.rand.NextFloat(1.1f, 3.8f));
                Vector2 dVelocity = new Vector2(4f, 0).RotatedBy(rotate * Main.rand.NextFloat(1.1f, 3.8f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + dPos, CIDustID.DustSandnado, new Vector2(dVelocity.X, dVelocity.Y));
                dust.noGravity = true;
                dust.velocity = dVelocity;
                dust.scale = Main.rand.NextFloat(0.8f, 1.1f);
            }
            //可以考虑生成一些小爆炸, 但这三王后的锤子要啥自行车?   
        }
    }
}