using CalamityInheritance.World;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using MonoMod.Cil;
using static MonoMod.Cil.ILContext;
using System.Collections.Generic;
using System.Reflection;
using System;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Utils;
using System.Linq;

namespace CalamityInheritance.System.ModeChange.Malice
{
    public class MaliceChange
    {
        public class MaliceModeProj : GlobalProjectile
        {
            #region 弹幕速度
            public override bool InstancePerEntity => true;
            public static bool BadProj(Projectile proj)
            {
                if (proj.active && proj.hostile && !proj.friendly)
                {
                    return proj.damage > 0;
                }
                return false;
            }

            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (BadProj(projectile) && CIWorld.Malice)
                    projectile.velocity *= 1.5f;
            }
            #endregion
        }
        public class MaliceModeNPC : GlobalNPC
        {
            #region 基础数据
            // 速度乘数
            private const float SpeedMultiplier = 1.25f;
            // 用于储存额外更新AI的进度
            private float _updateAccumulator;
            public override bool InstancePerEntity => true;
            #endregion
            #region 判定
            // 判定可以增加速度
            // 必须恶意才给
            public static bool ShouldAccelerate(NPC npc) => npc.active && !npc.friendly && !npc.CountsAsACritter && CIWorld.Malice;
            #endregion
            #region 加载与卸载IL和ON
            public override void Load()
            {
                IL_NPC.UpdateNPC_Inner += ModifyVelocityIL;
                On_NPC.UpdateNPC_Inner += AccelerateAIUpdates;
                On_NPC.UpdateCollision += HandleCollisionSpeed;
            }
            public override void Unload()
            {
                IL_NPC.UpdateNPC_Inner -= ModifyVelocityIL;
                On_NPC.UpdateNPC_Inner -= AccelerateAIUpdates;
                On_NPC.UpdateCollision -= HandleCollisionSpeed;
            }
            #endregion
            #region 注入修改速度
            // IL注入修改移动速度
            public static void ModifyVelocityIL(ILContext il)
            {
                var cursor = new ILCursor(il);
                // 定位到velocity计算的位置，定位的原理我不知道（
                if (cursor.TryGotoNext(MoveType.After,i => i.MatchLdfld<NPC>("velocity"),i => i.MatchCall<Vector2>("op_Addition")))
                {
                    cursor.Emit(OpCodes.Ldarg_0);
                    cursor.EmitDelegate<Func<Vector2, NPC, Vector2>>((originalVel, npc) => ShouldAccelerate(npc) ? originalVel * SpeedMultiplier : originalVel);
                }
            }
            #endregion
            #region 额外更新
            // 加速AI更新频率
            public void AccelerateAIUpdates(On_NPC.orig_UpdateNPC_Inner orig, NPC npc, int index)
            {
                if (ShouldAccelerate(npc))
                {
                    _updateAccumulator += SpeedMultiplier - 1f;
                    while (_updateAccumulator >= 1f)
                    {
                        PerformExtraUpdate(npc);
                        _updateAccumulator--;
                    }
                }
                orig(npc, index);
            }
            #endregion
            #region NPC的额外更新实现
            // NPC的额外更新
            public static void PerformExtraUpdate(NPC npc)
            {
                if (!npc.active) return;

                NPCLoader.ResetEffects(npc);
                if (npc.life <= 0)
                {
                    npc.active = false;
                    npc.justHit = false;
                }
                else
                {
                    npc.AI();
                    npc.CheckActive();
                    npc.justHit = false;
                }
            }
            #endregion
            #region 处理碰撞后的速度保留
            // 处理碰撞后的速度保留
            private void HandleCollisionSpeed(On_NPC.orig_UpdateCollision orig, NPC npc)
            {
                if (!ShouldAccelerate(npc))
                {
                    orig(npc);
                    return;
                }
                var originalSpeed = npc.velocity;
                npc.velocity *= SpeedMultiplier;
                orig(npc);

                // 保留不低于原始加速的速度
                npc.velocity = new Vector2(
                    Math.Sign(npc.velocity.X) * Math.Max(Math.Abs(npc.velocity.X), Math.Abs(originalSpeed.X)),
                    Math.Sign(npc.velocity.Y) * Math.Max(Math.Abs(npc.velocity.Y), Math.Abs(originalSpeed.Y))
                );
            }
            #endregion
        }
    }
}
   
