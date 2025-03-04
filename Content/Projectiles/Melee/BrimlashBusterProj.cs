using Microsoft.Build.Evaluation;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BrimlashBusterProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40; 
        }

        public override void AI()
        {
            //这武器除了贴图和攻击模板本身，可能跟之前也不是一个东西了
            //不过一把武器除了这两个以外，你也很难从其他地方看出不同点

            Projectile.ai[0] += 1f; //依旧是计时器的自增
            
        }
    }
}