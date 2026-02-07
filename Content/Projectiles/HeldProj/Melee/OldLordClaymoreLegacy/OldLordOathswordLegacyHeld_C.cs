using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Utilities;
using CalamityMod;
using LAP.Core.AnimationHandle;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.OldLordClaymoreLegacy
{
    public class OldLordOathswordLegacyHeld_C : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<OldLordOathswordLegacy>();
        public override string Texture => GetInstance<OldLordOathswordLegacy>().Texture;
        public Player Owner => Main.player[Projectile.owner];
        public AnimationHelper animationHelper = new(3);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            Projectile.Center = Owner.Center;
            Owner.CIMod().CanUseOldLordDash = false;
            Projectile.rotation += 0.4f;
            Projectile.rotation = MathHelper.WrapAngle(Projectile.rotation);
            Owner.fullRotation = Projectile.rotation;
            Owner.fullRotationOrigin = Owner.Center - Owner.position;
            Owner.SetImmuneTimeForAllTypes(15);


            for (int i = 0; i < 4; i++)
            {
                Vector2 dustSpawnPosition = Projectile.Center + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(Projectile.width) + Main.rand.NextVector2Circular(6f, 6f);
                Vector2 dustVelocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(6f) + Main.rand.NextVector2Circular(2.4f, 2.4f);

                Dust fire = Dust.NewDustPerfect(dustSpawnPosition, 6, dustVelocity);
                fire.scale = 1.7f;
                fire.noGravity = true;
            }
            if (Main.myPlayer == Projectile.owner && Projectile.timeLeft % 6f == 5f)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(8f, 8f), ProjectileType<OathswordFlame>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner);
            if (Main.myPlayer == Projectile.owner)
            {
                float VelMult = MathHelper.Lerp(0.2f, 1f, Projectile.timeLeft / 60f);
                Owner.velocity = Vector2.Lerp(Owner.velocity, Owner.SafeDirectionTo(Main.MouseWorld) * 32f * VelMult, 0.125f);
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Main.myPlayer);
                if (Owner.velocity.Y > 0)
                    Owner.LAP().NoSlowFall = 2;
            }
            if (Projectile.timeLeft < 2)
                Owner.fullRotation = 0f;
        }
        //public override void OnKill(int timeLeft) => Owner.fullRotation = 0f;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.PiOver2 + MathHelper.PiOver4 : MathHelper.PiOver4);
            Vector2 rotationPoint = Projectile.spriteDirection == -1 ? new Vector2(texture.Width, texture.Height) : new Vector2(0, texture.Height);
            SpriteEffects flipSprite = Projectile.spriteDirection * Main.player[Projectile.owner].gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture, drawPosition, null, Color.White, drawRotation, rotationPoint, Projectile.scale, flipSprite, 0f);
            return false;
        }
    }
}
