using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class TerraShard : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Dust terraMagic = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.TerraBlade, 0f, 0f, 0, default, 0.5f);
                terraMagic.scale = 0.42f;
                terraMagic.velocity *= 0.1f;
            }

            Projectile.HomeInNPC(1500f, 15f, 20f);
        }
    }
}
