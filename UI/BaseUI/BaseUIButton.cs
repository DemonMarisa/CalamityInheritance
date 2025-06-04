using CalamityInheritance.Content.Items.Placeables.Vanity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.UI.BaseUI
{
    // 还没完成，没有写统一的管理类，暂时别用（
    public class BaseUIButton
    {
        /// <summary>
        /// 缩放<br/>
        /// </summary>
        public virtual Vector2 Scale { get; set; } = new Vector2(1 , 1);

        /// <summary>
        /// 锚点<br/>
        /// </summary>
        public virtual Vector2 org { get; set; } = Vector2.Zero;

        /// <summary>
        /// 位置<br/>
        /// </summary>
        public virtual Vector2 Pos { get; set; } = Vector2.Zero;

        /// <summary>
        /// 旋转<br/>
        /// </summary>
        public virtual float rot { get; set; } = 0f;

        /// <summary>
        /// 颜色<br/>
        /// </summary>
        public virtual Color color { get; set; } = Color.White;

        /// <summary>
        /// 材质<br/>
        /// </summary>
        public virtual Texture2D Texture { get; set; } = null;

        /// <summary>
        /// 提供给缩放进度使用<br/>
        /// </summary>
        public virtual int FadeTime { get; set; } = 0;

        /// <summary>
        /// 淡入淡出最大值<br/>
        /// </summary>
        public virtual int FadeTimeMax { get; set; } = 30;

        /// <summary>
        /// 鼠标碰撞<br/>
        /// </summary>
        public Rectangle mouseRectangle = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
        /// <summary>
        /// 是否活跃<br/>
        /// </summary>
        public bool active = false;
        /// <summary>
        /// 绘制与更新<br/>
        /// </summary>
        public virtual void UpDateAndDraw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                if (FadeTime < FadeTimeMax)
                    FadeTime++;
            }
            else if (FadeTime > 0)
                FadeTime--;
            // 进度为0的时候就返回不处理
            if (FadeTime == 0)
                return;
            Update();
            UIDraw(spriteBatch);
        }
        /// <summary>
        /// 更新<br/>
        /// </summary>
        public virtual void Update()
        {
        }
        /// <summary>
        /// 绘制<br/>
        /// </summary>
        public virtual void UIDraw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
                spriteBatch.Draw(Texture, Pos, null, color, rot, org, Scale, SpriteEffects.None, 0f);
        }
    }
}
