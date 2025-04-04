using System;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.NPCs.Boss.Calamitas.Brothers
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
            if (!player.active || player.dead)
            {
                brother.TargetClosest(false);
                player = Main.player[brother.target];
                if (!player.active || player.dead)
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
        /// 这个唯一的作用只是初始化兄弟的索敌数据。
        /// 因为普灾与兄弟共用了一个追及AI[TryKeeping]，但他们用的数据是不同的
        /// 因此这个keepAngle就是专门给兄弟用的
        /// KeepAngle里面就调用了TryKeeping的方法，所以不需要再次调用了
        /// </summary>
        /// <param name="brother"></param>
        /// <param name="rotSpeed"></param>
        /// <param name="player"></param>
        /// <returns>转角</returns>
        public static float KeepAngle(ref NPC brother, float rotSpeed, Player player)
        {
            float bDistX = brother.position.X + brother.width / 2 - player.position.X - player.width / 2;
            float bDistY = brother.position.Y + brother.height - 59f - player.position.Y - player.height / 2;
            float broRot = (float)Math.Atan2(bDistY, bDistX) + MathHelper.PiOver2;
            broRot = TryKeeping(brother, rotSpeed, broRot);
            return broRot;
        }
        /// <summary>
        /// 保持视角，并返回一个角速度, 可以直接丢弃
        /// </summary>
        /// <param name="npc">兄弟</param>
        /// <param name="rotSpeed">兄弟最大转速</param>
        /// <param name="rotAngle"></param>
        public static float TryKeeping(NPC npc, float rotSpeed, float rotAngle)
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
            else if (npc.rotation > MathHelper.TwoPi)
                npc.rotation -= MathHelper.TwoPi;
            if (npc.rotation > rotAngle - rotSpeed && npc.rotation < rotAngle + rotSpeed)
                npc.rotation = rotAngle;
            return npc.rotation;
        }
        /// <summary>
        /// 追击敌方单位
        /// </summary>
        /// <param name="brother">兄弟</param>
        /// <param name="player">玩家</param>
        public static void ChansingTarget(ref NPC brother, Player player)
        {
            float broProjSpeedMax = 5f;
            float broProjAccel = 0.1f;
            //射弹朝向
            int broProjAttackDir = 1;
            if (brother.position.X + brother.width / 2 < player.position.X + player.width)
                broProjAttackDir = -1;
            //获取射弹与玩家的距离
            Vector2 projVec = new(brother.position.X + brother.width * 0.5f, brother.position.Y + brother.height * 0.5f);
            float projTarX = player.position.X + player.width / 2 + broProjAttackDir * 180 - projVec.X;
            float projTarY = player.position.Y + player.height / 2 - projVec.Y;
            float projTarDist = (float)Math.Sqrt(projTarX * projTarX + projTarY * projTarY);
            float speedUp = 0.5f;
            //原灾在这里整了10个if来复线加速度效果，这里直接改成普通的加速度了
            if (projTarDist > 300f && projTarDist < 900f)
                broProjSpeedMax += speedUp + 0.05f;
            //让boss的移动变得顺滑，这里类似于回旋镖的那个AI
            projTarDist = broProjSpeedMax / projTarDist;
            projTarX *= projTarDist;
            projTarY *= projTarDist;
            //追逐玩家
            CIFunction.TryCatchTraget(brother, projTarX, projTarY, broProjAccel);
        }
        /// <summary>
        /// 兄弟停止冲刺
        /// </summary>
        /// <param name="brother">兄弟</param>
        public static void StopCharge(ref NPC brother)
        {
            brother.velocity.X *= 0.93f;
            brother.velocity.Y *= 0.93f;
            if (brother.velocity.X > -0.1 && brother.velocity.X < 0.1)
                brother.velocity.X = 0f;
            if (brother.velocity.Y > -0.1 && brother.velocity.Y < 0.1)
                brother.velocity.Y = 0f;
        }
    }
}