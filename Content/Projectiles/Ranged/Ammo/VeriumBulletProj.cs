using CalamityMod.Projectiles;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace CalamityInheritance.Content.Projectiles.Ranged.Ammo
{
    public class VeriumBulletProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        private float speed = 0f;
        public int TargetIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public ref float AttackTimer => ref Projectile.ai[0];
        public bool IsHit
        {
            get => Projectile.ai[2] is 1f;
            set => Projectile.ai[2] = value ? 1f : 0f;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }
        //byd谁想的残影，我重画了
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Projectile.BaseProjPreDraw(tex, 4, lightColor);
            return false;
        }
        internal float _cacheProgress = 0f;
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        public override bool PreAI()
        {
            //wtf is this？
            ref float TrailingDust = ref Projectile.localAI[0];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction;
            TrailingDust += 1f;
            //常规粒子。
            if (TrailingDust > 4f)
            {
                if (!IsHit)
                {
                    if (Main.rand.NextBool())
                    {
                        int purple = Dust.NewDust(Projectile.position, 1, 1, DustID.PurpleCrystalShard, 0f, 0f, 0, default, 0.5f);
                        Main.dust[purple].alpha = Projectile.alpha;
                        Main.dust[purple].velocity *= 0f;
                        Main.dust[purple].noGravity = true;
                    }
                }
                else
                {
                    if (Projectile.Opacity > 0)
                        Projectile.Opacity -= 0.5f;
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 newVec = new(Projectile.Center.X - Projectile.velocity.X / 10f * i, Projectile.Center.Y - Projectile.velocity.Y / 10f * i);
                        Dust d = Dust.NewDustDirect(newVec, 1, 1, Main.rand.NextBool() ? DustID.PurpleCrystalShard : DustID.BlueCrystalShard);
                        d.noGravity = true;
                        d.velocity *= 0f;
                        d.position = newVec;
                        d.scale = _cacheProgress;
                        d.alpha = Projectile.alpha;
                    }
                    if (_cacheProgress < 1f)
                        _cacheProgress += 0.1f;
                }
            }

            if (AttackTimer > 0f)
                AttackTimer--;
            if (speed == 0f)
                speed = Projectile.velocity.Length();
            if (AttackTimer <= 0f && IsHit)
            {
                NPC target = Main.npc[TargetIndex];
                //没有目标重新在300f里面找新目标
                if(!target.active || !target.chaseable)
                    target = Projectile.FindClosestTarget(300f);
                //直接使用追踪即可 
                if (target is not null)
                {
                    Projectile.HomingNPCBetter(target, speed, 15f);
                    return false;
                }
            }
            return true;
        }
            
        

        public override bool? CanHitNPC(NPC target) => AttackTimer <= 0f && target.CanBeChasedBy(Projectile);

        public override bool CanHitPvp(Player target) => AttackTimer <= 0f;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            AttackTimer = 10f;
            Projectile.damage /= 2;
            if (target.life > 0)
            {
                Projectile.ai[1] = target.whoAmI;
                IsHit = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            AttackTimer = 10f;
            Projectile.damage /= 2;
        }
    }
}
