namespace CalamityInheritance.NPCs.YharonReborn
{
    /// <summary>
    /// 这里，只考虑一阶段的AI
    /// 我们，或者说我需要把一阶段的Boss与二阶段的Boss拆开来用，这样会方便处理一些
    /// 并且1.4tmod更新过后的WorldCondition也不得不这样用。
    /// 以及，进入二阶段标记的WorldContiion应当为玩家合成Dark Sun Ring之后，任意原灾的和旧版的。
    /// </summary>
    public class YharonRebornAIPhase1 
    {
        /*
        *TODO:
        *我们需要：基本框架，完善的阶段内容
        *尽管旧龙看起来有点简陋，但其AI的复杂程度与炼狱龙的AI复杂度是所差无几
        */
        #region 枚举Yharon一阶段的攻击方式
        public enum RebornAttacks
        {
            IdleMaster,             //挂机
            NormalCharge,           //普通冲刺
            FastCharge,             //快冲
            FlareTornadoSummon,     //生成火龙卷
            HomingBomb,             //生成弱追踪炸弹
            LineHomingFlare,        //生成线列追踪火焰弹
            SlowBulletHell,         //慢速弹幕炼狱
            FastBulletHell,         //快速弹幕炼狱
            FlaresRingSpawn,        //生成一圈火焰弹
            Teleport,               //传送，然后冲刺
        }
        #endregion
        #region 贴图动画枚举
        public enum RebornFrameDrawing
        {
            None,
            FlapWings,
            IdleWings,
            Roar,
            OpenMouth,
        }
        #endregion
        #region 攻击顺序列表
        //子阶段1: 普冲->普冲->快冲->慢速弹幕炼狱&追踪火球->挂机->普冲->普冲->火球环->龙卷
        public static readonly RebornAttacks[] SubphasePattern1 =
        [
            RebornAttacks.NormalCharge,
            RebornAttacks.NormalCharge,
            RebornAttacks.FastCharge,
            RebornAttacks.SlowBulletHell,
            RebornAttacks.HomingBomb,
            RebornAttacks.IdleMaster,
            RebornAttacks.NormalCharge,
            RebornAttacks.NormalCharge,
            RebornAttacks.FlaresRingSpawn,
            RebornAttacks.FlareTornadoSummon,
        ];

        public static readonly RebornAttacks[] SubphasePattern2 =
        [
            RebornAttacks.NormalCharge,
            RebornAttacks.FastCharge,
            RebornAttacks.FastCharge,
            RebornAttacks.FlaresRingSpawn,
            RebornAttacks.HomingBomb,
            RebornAttacks.LineHomingFlare,
            RebornAttacks.NormalCharge,
            RebornAttacks.FastCharge,
            RebornAttacks.FastBulletHell,
        ];
        #endregion
        public void ThisAI()
        {
            
        }
    }
}
