using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class EyeOfNightCell : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 10;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            if (!Main.dedServ && Projectile.velocity.Length() > 5f)
                Dust.NewDustPerfect(Projectile.Center, (int)CalamityDusts.SulphurousSeaAcid).noGravity = true;

            Projectile.StickyProjAI(3);
        }
        public override void OnKill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 10; i++)
                    Dust.NewDustDirect(Projectile.position, 36, 36, (int)CalamityDusts.SulphurousSeaAcid).noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => Projectile.ModifyHitNPCSticky(4);
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1f)
            {
                target.AddBuff(BuffID.CursedInferno, 120);
            }
        }
    }
}
