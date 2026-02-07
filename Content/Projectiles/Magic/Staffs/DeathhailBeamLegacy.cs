using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Staffs
{
    public class DeathhailBeamLegacy : BaseLaserbeamProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        private Color startingColor = new Color(119, 210, 255);
        private Color secondColor = new Color(247, 119, 255);
        public override float MaxScale => 0.7f;
        public override float MaxLaserLength => 1599.999999f;
        public override float Lifetime => 20f;
        public override Color LaserOverlayColor => CalamityUtils.ColorSwap(startingColor, secondColor, 0.9f);
        public override Color LightCastColor => LaserOverlayColor;
        public override Texture2D LaserBeginTexture => LAPTextureRegister.UltimaRayStart.Value;
        public override Texture2D LaserMiddleTexture => LAPTextureRegister.UltimaRayMid.Value;
        public override Texture2D LaserEndTexture => LAPTextureRegister.UltimaRayEnd.Value;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override bool PreAI()
        {
            // Initialization. Using the AI hook would override the base laser's code, and we don't want that.
            if (Projectile.localAI[0] == 0f)
            {
                if (Main.rand.NextBool())
                {
                    secondColor = new Color(119, 210, 255);
                    startingColor = new Color(247, 119, 255);
                }
            }
            return true;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<GodSlayerInferno>(), 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<GodSlayerInferno>(), 180);
    }
}
