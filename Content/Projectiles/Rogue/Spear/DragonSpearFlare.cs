using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class DragonSpearFlare : CIRogueProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //只有参考意义, 实际情况下AI会一直试图更新这个东西
            Projectile.timeLeft = 200;
            Projectile.width = 56;
            Projectile.height = 64;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.localAI[1] > 15f;
        public override void SendExtraAI(BinaryWriter writer)
        {
            for (int i = 0; i < 2; i++)
                writer.Write(Projectile.localAI[i]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < 2; i++)
                Projectile.localAI[i] = reader.ReadSingle();
        }
        public override void AI()
        {
            //转角。
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            //使射弹上升过程中受到重力影响
            if (Main.rand.NextBool(2))
                TrailDust();
            HomingAI();

        }
        public void HomingAI()
        {
            Projectile.velocity.Y += 0.28f;
            Projectile.velocity.X *= 0.99f;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 30f && Projectile.velocity.Y < 0f)
            {
                Projectile.velocity.Y += 0.42f;
            }
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 10f)
            {
                LAPUtilities.HomeInNPC(Projectile, 1800f, 24f, 20f);
            }
            if (Projectile.localAI[1] < 20f) Projectile.timeLeft = 200;
        }
        public void TrailDust()
        {
            for (int i = 0; i < 2; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Pixie, 0f, 0f, 0, default, 0.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 1f;
                d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Pixie, 0f, 0f, 100, default, 0.5f);
                Main.dust[d].velocity *= 1f;
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISoundID.SoundFlamethrower, Projectile.position);
            CIUtils.DustCircle(Projectile.position, 8, 0.5f, CIDustID.DustHeatRay, true, 4f);
        }
    }
}