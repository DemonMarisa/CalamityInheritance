using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class ElementalNebula : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public Vector2 ProjScale = new(0.5f, 0.5f);
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
            Projectile.Size = ProjScale;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 148;

        public override void AI()
        {
            int dType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Main.dust[dType].noGravity = true;
            Main.dust[dType].velocity *= 0f;
            if (Projectile.timeLeft < 190)
                LAPUtilities.HomeInNPC(Projectile, 3000f, 24f, 25f, null, !Projectile.tileCollide);
            Projectile.rotation += 0.12f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}