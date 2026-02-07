using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Summon;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class DiseasedPikeSpear : BaseSpearProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<DiseasedPikeLegacy>();
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = TrueMeleeDamageClass.Instance;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override float InitialSpeed => 3f;
        public override float ReelbackSpeed => 2.4f;
        public override float ForwardSpeed => 0.8f;
        public override void ExtraBehavior()
        {
            if (Projectile.LAP().FirstFrame)
            {
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                {
                    Player player = Main.player[Projectile.owner];
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vel = LAPUtilities.GetVector2(player.Center, player.LocalMouseWorld()).RotatedByRandom(0.2f) * Main.rand.Next(4, 7);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, vel, ProjectileType<PlagueSeeker>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            if (Main.rand.NextBool(4))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 4; i++)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * 0.25f, ModContent.ProjectileType<PlagueBeeSmall>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner);
                    Main.projectile[proj].extraUpdates += i;
                    Main.projectile[proj].DamageType = DamageClass.Melee;
                }
            }
        }
    }
}
