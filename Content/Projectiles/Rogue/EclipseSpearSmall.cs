using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CalamityInheritance.Utilities;
using CalamityMod;
using Humanizer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseSpearSmall : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        private readonly Queue<Vector2> _linePos = new Queue<Vector2>();
        //拖尾长度
        private const int LineLength = 10;
        //拖尾透明度
        private const float LineAlpha = 0.5f;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = 75 * Projectile.MaxUpdates;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 65;
        public override void AI()
        {
            //确实就三句话.
            Lighting.AddLight(Projectile.Center, 0.5f, 0.4f, 0.15f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            CIFunction.HomeInOnNPC(Projectile, true, 600f, 17f, 20f);

            //不断获取绘制拖尾的位置, 并使用消息队列通信
            _linePos.Enqueue(Projectile.Center);
        if (_linePos.Count > LineLength)
            _linePos.Dequeue();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //获取钱一个位置
            Vector2 prePos = _linePos.First();
            foreach(Vector2 curPos in _linePos.Skip(1))
            {
                // 计算线段颜色（渐变透明度）
                float alpha = LineAlpha* (LineLength- (curPos- prePos).Length()) / LineLength;
                Color c = Color.DarkOrange* alpha;
                //绘制拖尾
                Main.spriteBatch.DrawLineBetter(prePos, curPos, c, 2f); 
                prePos = curPos;
            }
            return base.PreDraw(ref lightColor);
        }
    }
}