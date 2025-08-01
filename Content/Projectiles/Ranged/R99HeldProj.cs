using System;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class R99HeldProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectile.Ranged";
        public override string Texture => $"{Generic.WeaponPath}/Ranged/R99";
        public Player Owner => Main.player[Projectile.owner];
        public const float OffsetX = 0f;
        public const float OffsetY = 0f;
        public enum AttackType : int
        {
            NotCrackedShield = 1,
            IsCrackedShield = 2,
            RechargingShield = 3
        };
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float AttackStyle => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 74;
            Projectile.width = 172;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.extraUpdates = 0;
            Vector2 offset = new(0, 0f);
            Vector2 rrp = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            //更新手持位置
            UpdateVisual(rrp);
            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAiming(rrp, Owner.HeldItem.shootSpeed);
                bool ifStillUse = (Owner.channel || Owner.controlUseTile) && !Owner.noItems && !Owner.CCed;
                if (ifStillUse)
                    HoldoutAI();
                else
                    DelCondition();
            }
            Projectile.timeLeft = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.Pi : 0);
            Vector2 rotationPoint = tex.Size() * 0.5f;
            SpriteEffects flipSprite = Projectile.spriteDirection * Main.player[Projectile.owner].gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(tex, drawPos, null, Projectile.GetAlpha(lightColor), drawRot, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite, default);
            return false;
        }
        private void DelCondition()
        {
            Projectile.Kill();
        }

        private void HoldoutAI()
        {
            RecoilAnimation();
            FiringBullet();
        }

        private void FiringBullet()
        {
            var src = Projectile.GetSource_FromThis();
            Vector2 fireDir = Vector2.UnitX.RotatedBy(Projectile.rotation);
            fireDir = fireDir.SafeNormalize(Vector2.UnitX);

            Owner.PickAmmo(Owner.ActiveItem(), out int Proj, out float shootSpeed, out int damage, out float kb, out int ammoID);
            Vector2 velocity = fireDir * shootSpeed;
            AttackTimer += 1f;
            if (AttackTimer % 1 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item41 with { MaxInstances = 0 }, Projectile.Center);
                int p = Projectile.NewProjectile(src, Projectile.Center, fireDir * shootSpeed, Proj, damage, kb, Projectile.owner);
                Main.projectile[p].CalamityInheritance().IfR99 = true;
                Main.projectile[p].extraUpdates += 4;
            }
        }

        private void RecoilAnimation()
        {
            //Shaking这个武器即可
            // Projectile.position += Main.rand.NextVector2Circular(1.4f, 1.4f);
        }

        private void UpdateAiming(Vector2 rrp, float shootSpeed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - rrp);
            if (aim.HasNaNs())
                aim = -Vector2.UnitY;
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 0.5f));
            aim *= shootSpeed;
            if (aim != Projectile.velocity)
                Projectile.netUpdate = true;
            Projectile.velocity = aim;
        }

        private void UpdateVisual(Vector2 rrp)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(Owner.direction);
            Projectile.Center = rrp;
            Projectile.spriteDirection = Projectile.direction;
            //刷新玩家的itemItem等让他持有射弹
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }
    }
}