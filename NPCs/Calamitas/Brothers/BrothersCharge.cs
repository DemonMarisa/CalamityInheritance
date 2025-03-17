using System.Numerics;
using CalamityInheritance.Utilities;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.NPCs.Calamitas.Brothers
{
    public class BrothersCharge
    {
        /// <summary>
        /// 初始化冲刺并进行冲刺
        /// </summary>
        /// <param name="brother">兄弟，即原始单位</param>
        /// <param name="player">玩家，即目标单位</param>
        /// <param name="rotAngle">角速度</param>
        /// <param name="chargeSpeed">冲刺速度，默认18f</param>
        public static void ChargeInit(NPC brother, Player player, float rotAngle, float? chargeSpeed = 18f)
        {
            brother.damage = brother.defDamage;
            SoundEngine.PlaySound(SoundID.Roar, brother.Center);
            brother.rotation = rotAngle;
            float getChargeSpeed = chargeSpeed.Value;
            if (chargeSpeed.HasValue) getChargeSpeed = chargeSpeed.Value;
            Vector2 chargeCenter = CIFunction.GetNpcCenter(brother);
            float chargeTarX = player.position.X + player.width / 2 - chargeCenter.X;
            float chargeTarY = player.position.Y + player.height / 2 - chargeCenter.Y;
            float chargeDist = CIFunction.TryGetVectorMud(chargeTarX, chargeTarY);
            chargeDist = getChargeSpeed / chargeDist;
            brother.velocity.X = chargeTarX * chargeDist;
            brother.velocity.Y = chargeTarY * chargeDist;
        }
    }
}