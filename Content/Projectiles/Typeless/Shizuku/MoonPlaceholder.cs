using System;
using CalamityInheritance.Content.Items.Weapons.Typeless;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Boss;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class MoonPlaceholder : ModProjectile, ILocalizedModType
    {
        public ref float Timer => ref Projectile.ai[0];
        
        public Player Owner => Main.player[Projectile.owner];
        public int OwnedProjectileType = ModContent.ProjectileType<ShizukuEdgeProjectileAlter>();
        public const float FloatingMoonDistance = 16f;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void AI()
        {
            
            DoReady();
            //这里会需要类似于虚空箭袋的逻辑
            DoRotated();
            //类虚空箭袋的增强
            DoBuffedCorrectedProj();
            //擦撞消弹的效果
            DoClearHostileProj();
        }

        public void DoReady()
        {
            //往AI里面不断检查是否应该生成这个射弹，如果不允许生成，则自动干掉自己
            if (Owner.HeldItem.type != ModContent.ItemType<ShizukuEdge>() || Owner.ownedProjectileCounts[OwnedProjectileType] < 1)
            {
                Projectile.Kill();
                Projectile.netUpdate = true;
                return;
            }
            //高举法器 -> 将法器投影至半空，并将月球放置在其之上而非绕身旋转（主要是为了消除任何可能存在的车万neta）
            //将加强射弹的功能默认设定为弹幕属性，明月

        }

        public void DoRotated()
        {
        }

        //Todo: 我应该需要用一种方法来降低开销...
        public void DoBuffedCorrectedProj()
        {
            //如果玩家没有激活武器，干掉下方的AI，不要执行
            //Todo: 这里改为检测手持射弹。
            if (Owner.ActiveItem().type != ModContent.ItemType<ShizukuEdge>() || Owner.dead)
                return;

            //加强类型
            foreach (var proj in Main.ActiveProjectiles)
            {
                Projectile p = proj;
                if (!CheckProj<SoulLargePlaceholder>(p) || !CheckProj<SoulMidPlaceholder>(p) || !CheckProj<SoulSmallPlaceholder>(p))
                    continue;
                if (!CheckProj<ScythePlaceholder>(p))
                    continue;
                //穿过的弹幕赋予ai2 = -1f，我们在这里不执行1.5倍增伤，因为我们会考虑其启用一个更新的AI
                if (Projectile.Hitbox.Intersects(p.Hitbox))
                    p.ai[2] = -1f;
                
            }
        }
        public void DoClearHostileProj()
        {
            //获取月球与玩家的距离
            float distance = (Projectile.Center - Owner.Center).Length();
            //略微往外扩展一点
            distance += CIFunction.SetDistance(20);
            //我们需要以这个距离排除掉不符合条件的射弹，减少开销
            foreach (var proj in Main.ActiveProjectiles)
            {
                Projectile possibleProj = proj;
                //排除距离之外的射弹
                if ((possibleProj.Center - Owner.Center).Length() > distance)
                    continue;
                //排除非敌伤害
                if (!possibleProj.hostile)
                    continue;
                //补特判：别把红月干掉了，不然终灾AI会直接鬼畜
                if (CheckProj<BrimstoneMonster>(possibleProj) || CheckProj<BrimstoneMonsterLegacy>(possibleProj))
                    continue;
                //查看是否接触玩家，如果是则舍去
                if (Projectile.Hitbox.Intersects(proj.Hitbox))
                {
                    proj.Kill();
                    //有些射弹可能不可消除，这里补一个active
                    proj.active = false;
                    //每一下成功的消弹都发送一个tint
                    SendTint();
                }
            }
        }

        public void SendTint()
        {
        }

        public static bool CheckProj<T>(Projectile proj) where T : ModProjectile => proj.type == ModContent.ProjectileType<T>();
        public bool CheckHeldProj<T>(Projectile proj) where T : ModProjectile => Owner.ownedProjectileCounts[proj.type] <= 0;
    }
}