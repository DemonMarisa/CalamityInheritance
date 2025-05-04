using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using CalamityInheritance.Utilities;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PhantasmalRuinGhost : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void SendExtraAI(BinaryWriter writer) => Projectile.DoSyncHandlerWrite(ref writer);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.DoSyncHandlerRead(ref reader);
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if (Projectile.timeLeft >= 207)
                Projectile.alpha -= 6;

            if (Projectile.timeLeft <= 25)
                Projectile.alpha += 9;
            CIFunction.HomeInOnNPC(Projectile, true, 1500f, 12f, 40f);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(100, 200, 255, 100);

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 239)
                return false;

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }
    }
}