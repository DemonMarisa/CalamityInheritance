using CalamityInheritance.Content.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Typeless.Heal
{
    public class NorHealthProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public ref float FlySpeed => ref Projectile.ai[0];
        public ref float Acceleration => ref Projectile.ai[1];
        public ref float HealAmt => ref Projectile.ai[2];
        public Player Healer => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            //默认300
            Projectile.timeLeft = 30000;
            //干掉不可穿墙
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            if (!Healer.active || Healer.dead || (Projectile.Center - Healer.Center).Length() > 3000f)
            {
                Projectile.netUpdate = true;
                Projectile.Kill();
                return;
            }
            CIFunction.HomeInPlayer(Healer, Projectile, 0f, FlySpeed, Acceleration);
            float distance = (Projectile.Center - Healer.Center).Length();
            if (Projectile.Hitbox.Intersects(Healer.Hitbox) || distance < 20f)
            {
                Projectile.netUpdate = true;
                Projectile.Kill();
            }
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
