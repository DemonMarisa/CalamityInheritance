using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Graphics.Lightning;
using LAP.Core.Presets.Content;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class ElementalLightning : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 End = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            float CheckPos = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, End, 24, ref CheckPos);
        }
        public override void AI()
        {
            Vector2 End = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            if (Projectile.LAP().FirstFrame)
            {
                LightningSetting setting = new LightningSetting(Projectile.Center, End, Color.Turquoise,
                    strength: 30,
                    width: 15,
                    lifetime: 60,
                    generationsStep: 7,
                    branchChance: 0.4f,
                    maxBranchGenerations: 3,
                    distanceProtect: 100,
                    strengthDecay: 0.6f,
                    maxBranchAllowedDistance: 1000);
                LightningBuilder.SpawnLightning(setting);
            }
            Projectile.velocity = Vector2.Zero;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
    }
}
