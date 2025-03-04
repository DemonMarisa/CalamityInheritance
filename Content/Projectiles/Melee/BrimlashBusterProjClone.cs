using Microsoft.Build.Evaluation;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BrimlashBusterProjClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityInheritance/Content/Projectiles/Melee/BrimlashBusterProj";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40; 
        }

        public override void AI()
        {
            //不管是哪个射弹，都会需要飞行一段时间    
            Projectile.ai[0] += 1f; //依旧是计时器的自增
        }
    }
}