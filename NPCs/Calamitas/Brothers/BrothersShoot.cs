using System.Numerics;
using CalamityInheritance.Utilities;
using Terraria;

namespace CalamityInheritance.NPCs.Calamitas.Brothers
{
    public class BrothersShoot
    {
        /// <summary>
        /// 发射指定的射弹
        /// </summary>
        /// <param name="npc">npc</param>
        /// <param name="player">玩家</param>
        /// <param name="pType">射弹类型</param>
        /// <param name="projSpeed">射弹速度</param>
        /// <param name="projDMG">射弹伤害</param>
        public static void JustShoot(NPC npc, Player player, int pType, float projSpeed, int projDMG)
        {
            Vector2 tarCenter = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
            float tarX = player.position.X + player.width /2 - tarCenter.X;
            float tarY = player.position.Y + player.height/2 - tarCenter.Y;
            float tarDist = CIFunction.TryGetVectorMud(tarX, tarY);
            tarDist = projSpeed / tarDist;
            tarX *= tarDist;
            tarY *= tarDist;
            tarX += npc.Center.X * 0.5f;
            tarY += npc.Center.Y * 0.5f;
            tarCenter.X -= tarX;
            tarCenter.Y -= tarY;
            Projectile.NewProjectile(npc.GetSource_FromAI(), tarCenter.X, tarCenter.Y, tarX, tarY, pType, projDMG, 0f, player.whoAmI, 0f, 0f);
        }
    }
}
