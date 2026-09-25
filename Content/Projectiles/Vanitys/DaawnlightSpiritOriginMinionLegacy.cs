using CalamityInheritance.Content.Items.Accessories.Vanitys;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Vanitys
{
    public class DaawnlightSpiritOriginMinionLegacy : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<DaawnlightSpiritOriginLegacy>();
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 146;
            Projectile.height = 164;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (!Owner.active)
            {
                Projectile.active = false;
                return;
            }
            if (!Owner.CI().CanHaveDaawnlightLegacy || Owner.dead)
            {
                Projectile.Kill();
            }
            DoMovement();
            HandleFrames();
        }
        public void DoMovement()
        {
            if (Projectile.WithinRange(Owner.Center, 100f))
                Projectile.velocity *= 0.975f;
            else
            {
                float flySpeed = MathHelper.Clamp(11f + Projectile.Distance(Owner.Center) * 0.015f, 11f, 25f);
                Projectile.velocity = Projectile.velocity.MoveTowards(LAPUtilities.GetVector2(Projectile.Center, Owner.Center) * flySpeed, flySpeed * 0.02f);
                if (!Projectile.WithinRange(Owner.Center, 2200f))
                {
                    Projectile.Center = Owner.Center;
                    Projectile.velocity = -Vector2.UnitY * 4f;
                    Projectile.netUpdate = true;
                }
            }

            if (MathHelper.Distance(Projectile.Center.X, Owner.Center.X) > 80f)
                Projectile.spriteDirection = (Projectile.Center.X > Owner.Center.X).ToDirectionInt();
        }

        public void HandleFrames()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 7 == 6)
                Projectile.frame = (Projectile.frame + 1) % 5;
        }
    }
}
