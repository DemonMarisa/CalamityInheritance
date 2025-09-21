using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalamityInheritance.Content.BaseClass
{
    public abstract class BaseHeldProj : ModProjectile
    {
        /// <summary>
        /// X 方向的偏移量<br/>
        /// </summary>
        public virtual float OffsetX { get; set; }

        /// <summary>
        /// Y 方向的偏移量<br/>
        /// </summary>
        public virtual float OffsetY { get; set; }

        /// <summary>
        /// Y 方向的基础偏移量，不会被朝向影响<br/>
        /// </summary>
        public virtual float BaseOffsetY { get; set; }
        /// <summary>
        /// 武器的旋转<br/>
        /// </summary>
        public virtual float WeaponRotation { get; set; }
        /// <summary>
        /// 武器转动的速度，越大越快<br/>
        /// </summary>
        public virtual float AimResponsiveness { get; set; }


        public int UseDelay = 0;
        //你小子jb给手持射弹创个新图片都不愿意用路径是吧
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            if (UseDelay > 0)
                UseDelay--;
            Projectile.extraUpdates = 0;

            Vector2 offset = new(0, BaseOffsetY);

            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true) - offset;
            player.heldProj = Projectile.whoAmI;
            // Update the Prism's position in the world and relevant variables of the player holding it.
            UpdatePlayerVisuals(player, rrp);

            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAim(rrp, player.HeldItem.shootSpeed);

                bool stillInUse = (player.channel || player.controlUseTile) && !player.noItems && !player.CCed;
                // Spawn in the Prism's lasers on the first frame if the player is capable of using the item.
                if (stillInUse)
                {
                    HoldoutAI();
                }
                else
                {
                    DelCondition();
                }
            }
            Projectile.timeLeft = 2;
            // 确保不会使用的时候消失
        }
        public virtual void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            // The beams emit from the tip of the Prism, not the side. As such, rotate the sprite by pi/2 (90 degrees).
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(WeaponRotation * player.direction);

            Vector2 offset = new Vector2(OffsetX, OffsetY * player.direction).RotatedBy(Projectile.rotation);
            Projectile.Center = playerHandPos + offset;

            Projectile.spriteDirection = Projectile.direction;

            // The Prism is a holdout Projectile, so change the player's variables to reflect that.
            // Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by Projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public virtual void UpdateAim(Vector2 source, float speed)
        {
            // Get the player's current aiming direction as a normalized vector.
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // Change a portion of the Prism's current velocity so that it points to the mouse. This gives smooth movement over time.
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        /// <summary>
        /// 手持弹幕的AI逻辑<br/>
        /// </summary>
        public virtual void HoldoutAI()
        {

        }
        /// <summary>
        /// 删除条件<br/>
        /// </summary>
        public virtual void DelCondition()
        {
            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ExtraPreDraw(ref lightColor);
            return false;
        }
        public virtual bool ExtraPreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.Pi : 0f);
            Vector2 rotationPoint = texture.Size() * 0.5f;
            SpriteEffects flipSprite = (Projectile.spriteDirection * Main.player[Projectile.owner].gravDir == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.spriteBatch.Draw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), drawRotation + MathHelper.ToRadians(7), rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite, default);
            MorePreDraw(ref lightColor);
            return false;
        }
        public virtual void MorePreDraw(ref Color lightColor)
        {
        }
    }
}
