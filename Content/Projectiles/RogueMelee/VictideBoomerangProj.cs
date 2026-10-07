using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Projectiles.Typeless.HomeIn;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class VictideBoomerangProj : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<VictideBoomerang>();
        public override string Texture => GetInstance<VictideBoomerang>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.extraUpdates = 1;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.CI().Stealth)
            {
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 800f, true);
                if (npc != null)
                {
                    Vector2 AimToTarget = npc.Center - Projectile.Center;
                    AimToTarget.Normalize();

                    if (Projectile.ai[0] % 8f == 0)
                    {
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, AimToTarget * 6f, ProjectileType<VictideShell>(), Projectile.damage / 2, Projectile.knockBack);
                        Main.projectile[p].SetStealthAttack();
                        Main.projectile[p].DamageType = Projectile.DamageType;
                    }
                }
                else if (Projectile.ai[0] % 8f == 0)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<VictideShell>(), Projectile.damage / 2, Projectile.knockBack);
                    Main.projectile[p].SetStealthAttack();
                    Main.projectile[p].DamageType = Projectile.DamageType;
                }
            }
            Player owner = Main.player[Projectile.owner];
            if (Projectile.ai[0] > 40f)
            {
                Projectile.PredictHomeIn(12f, 2f, owner);
                if (Projectile.Hitbox.Intersects(owner.Hitbox))
                    Projectile.Kill();
            }
            Projectile.rotation -= 0.22f;
        }
    }
}
