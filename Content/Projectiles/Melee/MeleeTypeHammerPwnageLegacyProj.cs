using System.Numerics;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class MeleeTypeHammerPwnageLegacyProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        private static readonly float RotationIncrement = 0.22f;
        private static readonly float StealthSpeed = MeleeTypeHammerPwnageLegacy.Speed*2;
        private static readonly int LifeTime = 240;
        private static readonly float ReboundTime = 45f;
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30 * Projectile.extraUpdates;
            Projectile.timeLeft = LifeTime;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);

            //锤子飞行时应当播报声音
            if(Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 10;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs,Projectile.position);
            }

            //继承至大部分锤子的ai: ai[0]存储锤子是(1f)否(0f)处于返程状态, 
            if(Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if(Projectile.ai[1] >= ReboundTime)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.tileCollide = false;
                float returnSpeed = MeleeTypeHammerPwnageLegacy.Speed;
                float acceleration = 1.6f;
                //返程
                CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                //接触玩家时kill
                if(Main.myPlayer == Projectile.owner)
                {
                    if(Projectile.Hitbox.Intersects(owner.Hitbox)) Projectile.Kill();
                }
            }
            //无论状态，锤子都应当在飞行过程中旋转
            Projectile.rotation += RotationIncrement;
            return;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //击中时造成神圣之火
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 240);
            OnHitDust();

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 600);
            OnHitDust();
        }
        private void OnHitDust() //击中时生成神圣粒子
        {
            float dCounts = 10f;
            float rotFactor = 360f/10f;
            for (int i = 0; i < dCounts; ++i)
            {
                float rot = MathHelper.ToRadians(i*rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 3.8f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 3.8f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, CIDustID.DustSandnado, new Vector2(velOffset.X, velOffset.Y));
                dust.noGravity = true;
                dust.velocity = velOffset;
                dust.scale = Main.rand.NextFloat(0.8f, 1.1f);
            }
            //可以考虑生成一些小爆炸, 但这三王后的锤子要啥自行车?   
        }
    }
}