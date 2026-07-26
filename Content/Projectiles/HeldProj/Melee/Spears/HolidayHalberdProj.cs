using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Projectiles.Melee.Spears;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class HolidayHalberdProj : BaseSpear
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<HolidayHalberd>();
        public override float RangeMin => 16;
        public override float RangeMax => 76;
        public override void ExAI()
        {
            if (Projectile.LAP().FirstFrame)
            {
                ShootProj();
            }
            GenDust();
        }
        public void ShootProj()
        {
            int damage = (int)(Projectile.damage * 0.5f);
            float kb = Projectile.knockBack * 0.5f;
            Vector2 projPos = Projectile.Center + Projectile.velocity;
            Vector2 projVel = Projectile.velocity.SafeNormalize(Vector2.One) * 12f;
            int type = Main.rand.NextBool(3) ? ProjectileType<GreenBall>() : ProjectileType<RedBall>();
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), projPos, projVel, type, damage, kb, Projectile.owner, 0f, 0f);
        }
        public void GenDust()
        {
            int dustType = 0;
            switch (Main.rand.Next(4))
            {
                case 1:
                    dustType = 107;
                    break;
                case 2:
                    dustType = 90;
                    break;
            }
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
            }
        }

    }
}
