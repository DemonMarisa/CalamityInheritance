using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Heal
{
    public class BlueHealProj : BaseHealProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void ExSD()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 3;
        }

        public override void ExKill()
        {
            for (int num136 = 0; num136 < 6; num136++)
            {
                Vector2 dspeed = Projectile.velocity * Main.rand.NextFloat(0.4f, 1.2f);
                float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * num136;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * num136;
                int num137 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.FrostHydra, 0f, 0f, 0, default, 1f);
                Main.dust[num137].alpha = Projectile.alpha;
                Main.dust[num137].position.X = x2;
                Main.dust[num137].position.Y = y2;
                Main.dust[num137].velocity = dspeed.RotatedByRandom(MathHelper.ToRadians(30));
                Main.dust[num137].noGravity = true;
            }
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

            Projectile.HomingTarget(Healer.Center, 3000, 18, 24);
            float distance = (Projectile.Center - Healer.Center).Length();
            if (Projectile.Hitbox.Intersects(Healer.Hitbox) || distance < 20f)
            {
                Projectile.netUpdate = true;
                Projectile.Kill();
            }

            for (int num136 = 0; num136 < 2; num136++)
            {
                Vector2 dspeed = -Projectile.velocity * Main.rand.NextFloat(0.5f, 0.7f);
                float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * num136;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * num136;
                int num137 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.FrostHydra, 0f, 0f, 0, default, 1f);
                Main.dust[num137].alpha = Projectile.alpha;
                Main.dust[num137].position.X = x2;
                Main.dust[num137].position.Y = y2;
                Main.dust[num137].velocity = dspeed;
                Main.dust[num137].noGravity = true;
            }
        }
    }
}
