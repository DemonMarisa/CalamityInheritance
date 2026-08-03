using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal.Rogue
{
    public class TrackingDiskLaserProj : CIRogueProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public float Time
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 600;
            Projectile.ArmorPenetration = 10;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.2f, 0.1f, 0f);

            Time++;
            if (Time >= 10f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.TheDestroyer, 0f, 0f, 160, default, 2f);
                    dust.position = Projectile.Center;
                    dust.velocity = Projectile.velocity;
                    dust.scale = Projectile.scale;
                    dust.noGravity = true;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.Resize(60, 60);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }
    }
}
