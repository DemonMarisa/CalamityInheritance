using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Projectiles.Melee.Flails;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class VictideSpearProj : BaseSpear
    {
        public override string Texture => GetInstance<VictideSpear>().Texture;
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<VictideSpear>();
        public override float RangeMin => 16;
        public override float RangeMax => 62;
        public override void ExAI()
        {
            if (Projectile.LAP().FirstFrame)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 fireVel = Projectile.velocity.RotatedByRandom(0.1f) * 3f * Main.rand.NextFloat(0.6f, 1f);
                    fireVel.Y = fireVel.Y - 1f;
                    Projectile.NewProj(ProjectileType<UrchinBallSpike>(), Projectile.Center, fireVel, 0.5f);
                }
            }
        }
    }
}
