using CalamityInheritance.Content.Items.Weapons.Melee.Specials;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Specials
{
    public class DevilsSunriseProjLegacy : ModProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<DevilsSunriseLegacy>();
        private int red;
        private const int greenAndBlue = 100;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.width = 148;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = TrueMeleeNoSpeedDamageClass.Instance;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Player Owner = Main.player[Projectile.owner];
            if (Projectile.ai[1] < 300f)
                Projectile.ai[1]++;

            Projectile.damage = (int)(Owner.GetTotalDamage(Projectile.DamageType).ApplyTo(Projectile.originalDamage) * (1f + Utils.GetLerpValue(0f, 300f, Projectile.ai[1], true)));

            red = 30 + (int)(Projectile.ai[1] * 0.75f);
            if (red > 255)
                red = 255;

            if (++Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;

            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
                Projectile.soundDelay = 24;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                if (!Owner.CantUseHoldout())
                {
                    float scaleFactor6 = 1f;

                    if (Owner.ActiveItem().shoot == Projectile.type)
                        scaleFactor6 = Owner.ActiveItem().shootSpeed * Projectile.scale;

                    Vector2 slashDirection = Main.MouseWorld - Owner.RotatedRelativePoint(Owner.MountedCenter, true);
                    slashDirection.Normalize();
                    if (slashDirection.HasNaNs())
                        slashDirection = Vector2.UnitX * (float)Owner.direction;

                    slashDirection *= scaleFactor6;
                    if (slashDirection.X != Projectile.velocity.X || slashDirection.Y != Projectile.velocity.Y)
                        Projectile.netUpdate = true;

                    Projectile.velocity = slashDirection;
                }
                else
                    Projectile.Kill();
            }

            Vector2 dustSpawn = Projectile.Center + Projectile.velocity * 3f;
            Lighting.AddLight(dustSpawn, red * 0.001f, 0.1f, 0.1f);

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(dustSpawn - Projectile.Size * 0.5f, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(red, greenAndBlue, greenAndBlue), 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position -= Projectile.velocity;
            }

            Projectile.position = Owner.RotatedRelativePoint(Owner.MountedCenter, true) - Projectile.Size * 0.5f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(red, greenAndBlue, greenAndBlue, Projectile.alpha);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<BrimstoneFlames>(), 180);
        }
    }
}
