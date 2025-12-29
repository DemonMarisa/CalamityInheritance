using CalamityMod;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal
{
    public class TrackingDiskLaserProj : RogueDamageProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.DraedonsArsenal";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public float Time
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void ExSD()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 600;
            Projectile.ArmorPenetration = 10;
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
            Projectile.ExpandHitboxBy(60);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }
    }
}
