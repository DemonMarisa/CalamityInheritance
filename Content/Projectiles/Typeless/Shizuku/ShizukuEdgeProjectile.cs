using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuEdgeProjectile : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuEdge";
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void AI()
        {
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //手动接管绘制
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle rec = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 ori = texture.Size() / 2f;
            SpriteEffects sE = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                sE = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture, drawPos, new Rectangle?(rec), lightColor, Projectile.rotation, ori, Projectile.scale, sE, 0);
            return false;
        }
    }
}