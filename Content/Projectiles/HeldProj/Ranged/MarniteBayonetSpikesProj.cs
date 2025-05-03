using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;
using static tModPorter.ProgressUpdate;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Ranged
{
    public class MarniteBayonetSpikesProj : BaseShortswordProjectile, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(32, 9);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>(); ;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 66 / 2;
            const int HalfSpriteHeight = 18 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public bool isFilp = false;
        public bool fireframe = false;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (!fireframe)
            {
                if (Main.MouseWorld.X < player.Center.X)
                    isFilp = true;
                else
                    isFilp = false;
                fireframe = true;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation  - (MathHelper.PiOver4 * Owner.direction);
            Vector2 rotationPoint = texture.Size() * 0.5f;
            SpriteEffects flipSprite = isFilp ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), drawRotation, rotationPoint, Projectile.scale * 0.5f, flipSprite);
            return false;
        }
    }
}
