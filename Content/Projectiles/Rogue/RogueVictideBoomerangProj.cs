using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Weapons.Melee.Boomerang;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueVictideBoomerangProj: ModProjectile, ILocalizedModType
    {
        public override string Texture => GetInstance<VictideBoomerangMelee>().Texture;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.Calamity().stealthStrike)
            {
                NPC npc = Projectile.FindClosestTarget(800f, true, true);
                if(npc != null)
                {
                    Vector2 AimToTarget = npc.Center - Projectile.Center;
                    AimToTarget.Normalize();

                    if (Projectile.ai[0] % 8f == 0)
                    {
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, AimToTarget * 6f, ProjectileType<VictideShell>(), Projectile.damage / 3, Projectile.knockBack);
                        Main.projectile[p].Calamity().stealthStrike = true;
                        Main.projectile[p].DamageType = GetInstance<RogueDamageClass>();
                    }
                }
                else if (Projectile.ai[0] % 8f == 0)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<VictideShell>(), Projectile.damage / 3, Projectile.knockBack);
                    Main.projectile[p].Calamity().stealthStrike = true;
                    Main.projectile[p].DamageType = GetInstance<RogueDamageClass>();
                }
            }
            Player owner = Main.player[Projectile.owner];
            float rSpeed = 16f;
            float accle = 1.5f;
            if (Projectile.ai[0] > 40f)
            {
                CIFunction.BoomerangReturningAI(owner, Projectile, rSpeed, accle);
                if (Projectile.Hitbox.Intersects(owner.Hitbox))
                    Projectile.Kill();
            }
            Projectile.rotation -= 0.22f;
        }
    }
}
