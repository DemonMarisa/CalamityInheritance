using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles
{
    public class CIElementalQuiverSplit : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        // 是否允许分裂
        public bool canSplit = true;
        public override void SetDefaults(Projectile projectile)
        {
            base.SetDefaults(projectile);

            if (CalamityInheritanceLists.rangedProjectileExceptionList.TrueForAll(x => projectile.type != x))
                canSplit = false;
        }
        public override void PostAI(Projectile projectile)
        {
            if (ElemQuiverCon(projectile))
            {
                ProjSpilt(projectile);
            }
        }

        public bool ElemQuiverCon(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();

            // 必须装备元素箭袋，必须是友好射弹，必须有伤害，必须是远程伤害，必须允许分裂

            if (!usPlayer.ElemQuiver)
                return false;

            if (!canSplit)
                return false;

            if (!projectile.friendly)
                return false;

            if (projectile.damage < 1)
                return false;

            if (projectile.DamageType != DamageClass.Ranged)
                return false;

            if (projectile.whoAmI == player.heldProj)
                return false;

            // 上面的判定都过了后过最终随机发射判定
            if (CIConfig.Instance.ElementalQuiverSplitstyle == 1 || CIConfig.Instance.ElementalQuiverSplitstyle == 2)
            {
                if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                    return true;
            }
            else
            {
                if (Main.rand.NextBool(100))
                    return true;
            }

            // 默认不分裂
            return false;
        }

        public static void ProjSpilt(Projectile projectile)
        {
            float spread = 180f * 0.0174f;
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
            {
                int projectile2 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                int projectile3 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                Main.projectile[projectile2].DamageType = DamageClass.Default;
                Main.projectile[projectile3].DamageType = DamageClass.Default;
                Main.projectile[projectile2].noDropItem = true;
                Main.projectile[projectile3].noDropItem = true;

                if (CIConfig.Instance.ElementalQuiverSplitstyle == 1 || CIConfig.Instance.ElementalQuiverSplitstyle == 3)
                {
                    Main.projectile[projectile2].timeLeft = 60;
                    Main.projectile[projectile3].timeLeft = 60;
                }

            }
        }
    }
}
