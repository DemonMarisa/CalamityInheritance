using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Projectiles.Melee.Spears;
using CalamityInheritance.Content.Projectiles.Typeless.HomeIn;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class DiseasedPikeSpear : BaseSpear
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<DiseasedPikeLegacy>();
        public override float RangeMin => 16;
        public override float RangeMax => 96;
        public override void ExAI()
        {
            if (Projectile.LAP().FirstFrame)
            {
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                {
                    Player player = Main.player[Projectile.owner];
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vel = LAPUtilities.GetVector2(player.Center, player.LocalMouseWorld()).RotatedByRandom(0.2f) * Main.rand.Next(4, 7);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, vel, ProjectileType<PlagueSeekerLegacy>(),
                            Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            if (Main.rand.NextBool(4))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIPlague>(), 300);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 4; i++)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * 0.25f,
                        ProjectileType<PlagueBeeLegacy>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner);
                    Main.projectile[proj].extraUpdates += i;
                    Main.projectile[proj].DamageType = DamageClass.Melee;
                }
            }
        }
    }
}
