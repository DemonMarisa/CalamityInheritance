using CalamityInheritance.Utilities;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Heal
{
    public class GlobalHealthProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        #region 别名
        public ref float FlySpeed => ref Projectile.ai[0];
        public ref float Acceleration => ref Projectile.ai[1];
        public ref float HealAmt => ref Projectile.ai[2];
        public Player Healer => Main.player[Projectile.owner];
        #endregion
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            //默认300
            Projectile.timeLeft = 30000;
            //干掉不可穿墙
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            //直接追踪锁定玩家位置就行了。我也不知道为什么要做别的事情。
            //人都似了为什么还要跑这个弹幕？
            //距离玩家过远也直接处死这个弹幕，没得玩的
            if (!Healer.active || Healer.dead || (Projectile.Center - Healer.Center).Length() > 3000f)
            {
                Projectile.netUpdate = true;
                Projectile.Kill();
                return;
            }
            //设置回旋镖AI 
            Healer.HomeInPlayer(Projectile, 20f, FlySpeed, Acceleration);
            float distance = (Projectile.Center - Healer.Center).Length();
            if (Projectile.Hitbox.Intersects(Healer.Hitbox) || distance < 50f)
            {
                //干掉射弹即可
                Projectile.netUpdate = true;
                Projectile.Kill();
            }
            //设置粒子，与颜色
            for (int i = 0; i < 3; i++)
            {
                int dustType = Main.rand.NextBool(4) ? 182 : (int)CalamityDusts.Brimstone;
                Vector2 dustSpawnPos = Projectile.position - Projectile.velocity * i / 2f;
                Dust crimtameMagic = Dust.NewDustPerfect(dustSpawnPos, dustType);
                crimtameMagic.scale = Main.rand.NextFloat(0.96f, 1.04f) * MathHelper.Lerp(1f, 1.7f, 0.2f);
                crimtameMagic.noGravity = true;
                crimtameMagic.velocity *= 0.1f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            //根据提供的恢复量给予治疗
            Healer.Heal((int)HealAmt);
        }
    }

}