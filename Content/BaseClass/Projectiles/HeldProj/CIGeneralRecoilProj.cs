using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityInheritance.Content.BaseClass.Projectiles.HeldProj
{
    /// <summary>
    /// 一个我经常使用的后坐力射弹
    /// <br>你可以通过输入特定的物品ID来直接让武器画成手持射弹的样子</br>
    /// <br>使用这个可以在不新建一大堆手持射弹的情况下仍然像物品一样操作，而且不会像使用<see cref="CIUtils.NoHeldProjUpdateAim(Player, float, float)"/>一样在转向时出现闪帧</br>
    /// <br>具体使用案例见<see cref="VesuviusLegendary"/></br>
    /// </summary>
    public class CIGeneralRecoilProj : CITypelessProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        /// <summary>
        /// 初始化持有物数据，包括枪械类型、后坐力、生命周期和持有偏移量
        /// </summary>
        /// <param name="gunType">指定枪械类型，这个输入的是物品ID</param>
        /// <param name="recoilPower">指定后坐力强度，为0时不会有后坐力</param>
        /// <param name="lifeTime">指定物品生命周期，通常可以输入<see cref="Item.useAnimation"/></param>
        /// <param name="holdoutoffset">指定持有物的偏移量。</param>
        public void SetUpHoldoutData(int gunType, float recoilPower, int lifeTime, Vector2 holdoutoffset)
        {
            ItemLifeTime = lifeTime;
            GunItemType = gunType;
            RecoilPower = recoilPower;
            HoldProjOffset = holdoutoffset;
        }
        /// <summary>
        /// 这把后坐力武器的”枪“的ID
        /// <br>用于给予后坐力的贴图</br>
        /// </summary>
        public int GunItemType = ItemID.BeeGun;
        public ref float RecoilPower => ref Projectile.ai[0];
        public int ItemLifeTime
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public Vector2 HoldProjOffset = Vector2.Zero;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.timeLeft = 100000;
        }
        public override void AI()
        {
            int duration = Owner.itemAnimationMax; // Define the duration the projectile will exist in frames

            // Reset projectile time left if necessary
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            Projectile.velocity = Owner.GetPlayerToMouseVector2();
            Projectile.rotation = Projectile.velocity.ToRotation();
            int dir = Math.Sign(Owner.LocalMouseWorld().X - Owner.Center.X);
            Projectile.spriteDirection = dir;
            Owner.heldProj = Projectile.whoAmI; // Update the player's held projectile id
            Owner.ChangeDir(dir);
            Owner.SetArmRot(Projectile.rotation);

            if (RecoilPower != 0)
            {
                float pullBack = RecoilPower;
                float animationProgress = 0.5f - Owner.itemTime / (float)Owner.itemTimeMax;
                if (animationProgress < .4f)
                    pullBack -= 2.75f * (float)Math.Pow((.6f - animationProgress) / .6f, 2f);
                Projectile.Center = Owner.MountedCenter + Projectile.rotation.ToRotationVector2() * pullBack;
            }
            else
                Projectile.Center = Owner.MountedCenter;
            Projectile.position.Y += Owner.gfxOffY;

        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Item[GunItemType].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 offset = HoldProjOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation;
            if (Projectile.spriteDirection < 0)
            {
                drawRot += MathHelper.Pi;
            }
            drawPos += offset.RotatedBy(drawRot);
            Vector2 rotPoint = tex.Size() / 2f;
            SpriteEffects se = (Projectile.spriteDirection * Owner.gravDir) == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(tex, drawPos, null, Color.White, drawRot, rotPoint, Projectile.scale, se, 0);
            return false;
        }
    }
}
