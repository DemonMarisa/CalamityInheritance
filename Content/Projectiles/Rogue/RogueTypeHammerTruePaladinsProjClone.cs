using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.CalPlayer;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTruePaladinsProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        private static float RotationIncrement = 0.15f;

        private static readonly int Lifetime = 200; //最后一次削减挂载时间,
        private static readonly float CanHome = 40;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);

            // The hammer makes sound while flying.
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }

            //真圣骑士锤的潜伏是同时投掷一把返程后追踪的锤子和一把挂载锤子, 因此不需要受到任何的重力影响
            //但是, 掷出后他仍然以一个非常夸张的速度减速
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.velocity.X *= 0.94f;
            Projectile.velocity.Y *= 0.94f;
            Projectile.ai[0] += 1f;
            if(Projectile.ai[0] == CanHome - 5)
            {
                Projectile.rotation += RotationIncrement * 0.2f;//大幅度缩短锤子转速
                SignalSend();
            }
            if(Projectile.ai[0] > CanHome) //使锤子跟踪, 需注意的是, 跟踪有较大的惯性
            {   
                Projectile.rotation += RotationIncrement + 5f;//增加转速
                CIFunction.HomeInOnNPC(Projectile, true, 3000f, 32f, 24f, 20f);
            }
            else
            Projectile.timeLeft = Lifetime; //允许跟踪前会刷新锤子的存续时间
            Projectile.rotation += RotationIncrement;//大锤子的旋转增长速度比他下位的锤子更慢

            //同时也会生成粒子
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(10, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(2, 0).RotatedBy(offset.ToRotation());
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.SolarFlare, new Vector2(Projectile.velocity.X * 0.2f + velOffset.X, Projectile.velocity.Y * 0.2f + velOffset.Y), 100, default, 0.8f);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.GemRuby, new Vector2(Projectile.velocity.X * 0.15f + velOffset.X, Projectile.velocity.Y * 0.15f + velOffset.Y), 100, default, 0.8f);
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 180); //真圣骑士锤现在对敌怪造成的是破晓, 而非硫磺火
            OnHitEffect();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
            {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240); //对玩家则造成硫磺火
            OnHitEffect();
        }

        public void SignalSend()
        {

            SoundEngine.PlaySound(SoundID.Item4 with {Volume = 0.3f}, Projectile.Center); //即将开始追踪前, 播报落星的声音
            CIFunction.DustCircle(Projectile.Center, 20f, 2.2f, DustID.GemRuby, true, 4.8f);
        }

        private void OnHitEffect()
        {
            SoundEngine.PlaySound(SoundID.Item89 with {Volume = 0.5f}, Projectile.Center);
            Player calPlayer = Main.player[Projectile.owner];
            calPlayer.Calamity().rogueStealth += 0.01f;
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                if (proj.WithinBounds(Main.maxProjectiles))
                    Main.projectile[proj].DamageType = ModContent.GetInstance<RogueDamageClass>();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
