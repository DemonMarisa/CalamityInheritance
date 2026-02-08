using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CalamityInheritance.Rarity.Special.RarityDrawHandler
{
    /// <summary>
    ///由炼狱启发，并结合了粒子系统去使用
    ///这里的粒子系统是单独写的一个新类，因为不需要modsystem接入管理，所以可以省一点加载时间
    ///尽管如此，仍然复制了一个BlendstateID，但是视情况来说可以移除。
    /// </summary>
    public abstract class RaritySparkle
    {
        /// <summary>
        /// 使用材质
        /// </summary>
        public virtual string Texture => (GetType().Namespace + "." + GetType().Name).Replace('.', '/');

        /// <summary>
        /// 该粒子存在了多少帧，一般不需要手动修改这个值
        /// </summary>
        public int Time = 0;

        /// <summary>
        /// 粒子的存在时间上限
        /// </summary>
        public int Lifetime = 0;

        /// <summary>
        /// 位置与向量
        /// </summary>
        public Vector2 Position;
        public Vector2 Velocity;

        public Vector2 Origin;
        public Color DrawColor;
        public float Rotation;
        public float Scale = 1f;

        /// <summary>
        /// 粒子的透明度
        /// </summary>
        public float Opacity = 1f;

        /// <summary>
        /// 生命周期的进度，介于0到1之间。
        /// 0表示粒子刚生成，1表示粒子消失。
        /// </summary>
        public float LifetimeRatio => Time / (float)Lifetime;

        /// <summary>
        /// 渲染的混合模式，默认为<see cref="BlendState.AlphaBlend"/>.
        /// </summary>
        public virtual int UseBlendStateID => BlendStateID.Alpha;
        public float TimeLeft => Lifetime - Time;
        public virtual void CustomUpdate() { }

        public virtual void CustomDraw(SpriteBatch spriteBatch, Vector2 drawPosition) { }
    }
}
