using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.Magic.GreatStaff;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.GreatStaff
{
    public class SparkInfernalold : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
        }

        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            int infernadoDamage = (int)player.GetTotalDamage<MagicDamageClass>().ApplyTo(TheWandold.BaseDamage);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<InfernadoMarkFriendlyold>(), infernadoDamage, Projectile.knockBack, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIDragonfire>(), 120);
        }
    }
}
