using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ScythePlaceholder : ModProjectile, ILocalizedModType
    {
        public ref float BuffPing => ref Projectile.ai[2];
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override bool PreAI()
        {
            //如果ai2 != -1f，不执行preAI
            if (BuffPing != -1f)
                return true;
                
            return false;
        }
        public override void AI()
        {
            // //高速射弹即可，采用类似南瓜王镰刀的AI
            // Projectile.rotation += MathHelper.PiOver2 + 
            // base.AI();
        }
    }
}