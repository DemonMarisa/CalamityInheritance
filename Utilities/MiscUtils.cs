using System;
using System.Numerics;
using Microsoft.Build.Construction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
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
        /// <summary>
        /// 指定区域寻找特定物品
        /// </summary>
        /// <param name="p">玩家</param>
        /// <param name="tar">指定物品</param>
        /// <param name="area">区域，1: 背包 2: 盔甲栏</param>
        /// <returns>真: 寻找成功</returns>
        public static bool FindInventoryItem(ref Player p, int tar, int area)
        {
            const int isInventory = 1;
            const int isArmor = 2;
            bool flag = false;
            switch (area)
            {
                case isInventory:
                    for (int i = 0; i < p.inventory.Length; i++)
                    {
                        if (p.inventory[i].type == tar)
                            flag = true;
                    }
                    break;
                case isArmor:
                    for (int i = 0; i < p.armor.Length; i++)
                        if (p.inventory[i].type == tar)
                            flag = true;
                    break;
            }
            return flag;
        }
        /// <summary>
        /// Extension which initializes a ModTile to be a trophy.
        /// </summary>
        /// <param name="mt">The ModTile which is being initialized.</param>
        internal static void SetUpTrophy(this ModTile mt)
        {
            // TODO -- how to force trophy drops correctly? they all have zero code in them

            Main.tileFrameImportant[mt.Type] = true;
            Main.tileLavaDeath[mt.Type] = true;
            Main.tileSpelunker[mt.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(mt.Type);
            TileID.Sets.DisableSmartCursor[mt.Type] = true;
            TileID.Sets.FramesOnKillWall[mt.Type] = true;

            mt.AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
            mt.DustType = 7;
        }
    }
}
