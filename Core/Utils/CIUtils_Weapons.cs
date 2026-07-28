using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static void BetterSwing(this Player player)
        {
            float xOffset = 6f;
            float yOffset = -10f;
            if (player.itemAnimation < player.itemAnimationMax * 0.333f)
                yOffset = 4f;
            else if (player.itemAnimation >= player.itemAnimationMax * 0.666f)
                xOffset = -4f;
            player.itemLocation.X = player.Center.X + xOffset * player.direction;
            player.itemLocation.Y = player.MountedCenter.Y + yOffset;
            if (player.gravDir < 0)
                player.itemLocation.Y = player.Center.Y + (player.position.Y - player.itemLocation.Y);
        }
        /// <summary>
        /// 让原版的手持也可以像手持弹幕一样旋转<br/>
        /// 随便找一个每帧调用的方法调用即可<br/>
        /// </summary>
        public static void NoHeldProjUpdateAim(Player player, float rotationOffset = 0f, float rotationSpeed = 1f)
        {
            player.ChangeDir(Math.Sign((player.LocalMouseWorld() - player.Center).X));

            Vector2 aimVect = player.LocalMouseWorld() - player.Center;
            aimVect.SafeNormalize(Vector2.UnitX);

            float targetRotation = aimVect.ToRotation();

            if (player.LocalMouseWorld().X < player.Center.X)
                player.itemRotation = player.itemRotation.AngleLerp(targetRotation - MathHelper.ToRadians(rotationOffset) + MathHelper.Pi, rotationSpeed);
            else
                player.itemRotation = player.itemRotation.AngleLerp(targetRotation + MathHelper.ToRadians(rotationOffset), rotationSpeed);
        }
    }
}
