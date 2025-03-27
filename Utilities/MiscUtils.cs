using System;
using System.Numerics;
using Microsoft.Build.Construction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Utilities
{
    /// <summary>
    /// get向量数据
    /// </summary>
    
    public static partial class CIFunction
    {
        struct GetVectorDistance
        {
            public float vector2Distance;
            public Vector2 NpcCenter;
            public Vector2 PlayerCenter;
        }   
        public static int SecondsToFrames(int seconds) => seconds * 60;
        public static int SecondsToFrames(float seconds) => (int)(seconds * 60);
        /// <summary>
        /// 输入一个>0的整形数, 返回对应的刻度, 一般而言60刻 = 1秒, 推荐使用时尽可能不要取0的值
        /// </summary>
        /// <param name="Sec">需要转化的秒数</param>
        /// <returns>返回一个浮点数，记录刻度</returns>
        public static float SecConvertTicks(int Sec)
        {
            return (Sec * 60);
        }
        /// <summary>
        /// 获取npc的“正中心”位置
        /// </summary>
        /// <param name="npc">npc</param>
        /// <returns>一个位于npc正中心的向量起点(或者终点，看你想怎么使用)</returns>
        public static Vector2 GetNpcCenter(NPC npc)
        {
            Vector2 npcPos = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height* 0.5f);
            return npcPos;
        }
        public static float TryGetVectorMud(float distanceX, float distanceY)
        {
            return (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
        }
        /// <summary>
        /// 播放射弹帧图
        /// </summary>
        /// <param name="projectile">射弹</param>
        /// <param name="fCounter">计时器，即间隔多少时间播放下一张帧图</param>
        /// <param name="fMax">这个帧图最大的帧数</param>
        public static int FramesChanger(Projectile projectile, int fCounter, int fMax) 
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > fCounter)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= fMax)
                projectile.frame = 0;
            return projectile.frame;
        }
    }
}
