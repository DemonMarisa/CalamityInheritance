using CalamityInheritance.NPCs;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        public static CIGlobalNPC CIMod(this NPC npc)
        {
            return npc.GetGlobalNPC<CIGlobalNPC>();
        }
        /// <summary>
        /// 这个用来去找玩家附近是否存在这个NPC
        /// </summary>
        /// <param name="npcType">NPC单位</param>
        /// <param name="player">玩家单位</param>
        /// <param name="range">最大检索距离</param>
        /// <returns>真:表示有这个npc</returns>
        public static bool IsThereNpcNearby(int npcType, Player player, float range)
        {
            if (npcType <= 0)
                return false;
            int npcIndex = NPC.FindFirstNPC(npcType);
            if (npcIndex != -1)
            {
                NPC npc = Main.npc[npcIndex];
                return npc.active && npc.Distance(player.Center) <= range;
            }
            return false;
        }
        /// <summary>
        /// 使原始单位能够追上你需要的目标单位
        /// 例：大部分boss的追逐AI。boss作为原始单位，玩家作为目标单位
        /// 则这个方法可以用于快速设置boss追赶的速度。
        /// !!!请务必确保这里面的变量是一定可变的
        /// </summary>
        /// <param name="npc">原始单位</param>
        /// <param name="tarSpeedX">目标单位的水平速度</param>
        /// <param name="tarSpeedY">目标单位的垂直速度</param>
        /// <param name="acceleration">当前单位具备的加速度</param>
        public static void TryCatchTraget(NPC npc, float tarSpeedX, float tarSpeedY, float acceleration)
        {
            if (npc.velocity.X < tarSpeedX)
            {
                npc.velocity.X += acceleration;
                if (npc.velocity.X < 0f && tarSpeedX > 0f)
                    npc.velocity.X += acceleration;
            }
            else if (npc.velocity.X > tarSpeedX)
            {
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > 0f && tarSpeedX < 0f)
                    npc.velocity.X -= acceleration;
            }
            if (npc.velocity.Y < tarSpeedY)
            {
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < 0f && tarSpeedY > 0f)
                    npc.velocity.X += acceleration;
            }
            else if (npc.velocity.Y > tarSpeedY)
            {
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > 0f && tarSpeedY < 0f)
                    npc.velocity.Y -= acceleration;
            }
        }
        /// <summary>
        /// 使你指定的npc单位发光R!G!B!
        /// </summary>
        /// <param name="npc">指定npc</param>
        /// <param name="red">R!</param>
        /// <param name="green">G!</param>
        /// <param name="blue">B!</param>
        public static void SetGlow(NPC npc, float red, float green, float blue)
        {
            float lightingPosX = (npc.position.X + npc.width/2)/16;
            float lightingPosY = (npc.position.Y + npc.width/2)/16;
            Lighting.AddLight(new Vector2(lightingPosX, lightingPosY), red, green, blue);
        }
    }
}
