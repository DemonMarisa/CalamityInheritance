using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class VividClarityBeam : CIExoProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Vector2 rotateVector = new Vector2(5f, 10f);
            if (Projectile.position.Y > Main.player[Projectile.owner].position.Y - 50f)
            {
                Projectile.tileCollide = true;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 48f)
            {
                Projectile.ai[0] = 0f;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustRotation = Vector2.UnitX * -12f;
                    dustRotation = -Vector2.UnitY.RotatedBy((double)(Projectile.ai[0] * 0.1308997f + (float)i * 3.14159274f), default) * rotateVector - Projectile.rotation.ToRotationVector2() * 10f;
                    int exo = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[exo].scale = 0.75f;
                    Main.dust[exo].noGravity = true;
                    Main.dust[exo].position = Projectile.Center + dustRotation;
                    Main.dust[exo].velocity = Projectile.velocity;
                }
            }

            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 49f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 300);
                if (target is not null)
                {
                    Vector2 fire = LAPUtilities.GetVector2(Projectile.Center, target.Center) * 6f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, fire, ModContent.ProjectileType<VividBolt>(), (int)(Projectile.damage * 0.66f), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 projPos = Projectile.position;
                    projPos -= Projectile.velocity * ((float)j * 0.25f);
                    int exod = Dust.NewDust(projPos, 1, 1, DustID.RainbowTorch, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[exod].noGravity = true;
                    Main.dust[exod].position = projPos;
                    Main.dust[exod].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[exod].velocity *= 0.1f;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CIMiracleBlight>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CIMiracleBlight>(), 300);
        }
    }
}
