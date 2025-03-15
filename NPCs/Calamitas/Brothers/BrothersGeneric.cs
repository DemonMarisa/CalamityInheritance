using System;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.NPCs.Calamitas.Brothers
{
    public class BrothersGeneric
    {
        /// <summary>
        /// 使兄弟消失
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="brother">兄弟</param>
        public static void BrothersDespawns(Player player, NPC brother)
        {
            if(!player.active || player.dead)
            {
                brother.TargetClosest(false);
                player = Main.player[brother.target];
                if(!player.active || player.dead)
                {
                    if (brother.velocity.Y > 3f)
                        brother.velocity.Y = 3f;
                    brother.velocity.Y -= 0.1f;
                    if (brother.velocity.Y < -12f)
                        brother.velocity.Y = -12f;
                    
                    if (brother.timeLeft > 60)
                        brother.timeLeft = 60;
                    
                    if (brother.ai[1] != 0f)
                    {
                        brother.ai[1] = 0f;
                        brother.ai[2] = 0f;
                        brother.ai[3] = 0f;
                        brother.netUpdate = true;
                    }
                    return;
                }
            }
        } 
        /// <summary>
        /// 保持视角，并返回一个角速度, 可以直接丢弃
        /// </summary>
        /// <param name="npc">兄弟</param>
        /// <param name="rotSpeed">兄弟最大转速</param>
        /// <param name="rotAngle"></param>
        public static float KeepAngle(NPC npc, float rotSpeed, float rotAngle)
        {
            
            if (rotAngle < 0f)
                rotAngle += MathHelper.TwoPi;
            else if (rotAngle > MathHelper.TwoPi)
                rotAngle -= MathHelper.TwoPi;
            
            if (npc.rotation < rotAngle)
            {
                if (rotAngle - npc.rotation > MathHelper.Pi)
                    npc.rotation -= rotSpeed;
                else
                    npc.rotation += rotSpeed;
            }
            else if (npc.rotation > rotAngle)
            {
                if (npc.rotation - rotAngle > MathHelper.Pi)
                    npc.rotation += rotSpeed;
                else
                    npc.rotation -= rotSpeed;
            }

            if (npc.rotation > rotAngle - rotSpeed && npc.rotation < rotAngle + rotSpeed)
                npc.rotation = rotAngle;
            if (npc.rotation < 0f)
                npc.rotation += MathHelper.TwoPi; 
            else if(npc.rotation > MathHelper.TwoPi)
                npc.rotation -= MathHelper.TwoPi;
            if(npc.rotation > rotAngle - rotSpeed && npc.rotation < rotAngle + rotSpeed)
                npc.rotation = rotAngle;
            return npc.rotation;
        }
    }
}