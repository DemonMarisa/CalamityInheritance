using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.FutureContent.JavelinHarpoon
{
    public class Insidiousjavelin : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<InsidiousImpaler>();
        public override string Texture => GetInstance<InsidiousImpalerLegacy>().Texture;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 8;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.scale = 1.3f;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20 * Projectile.extraUpdates;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] >= 60f)
                {
                    Projectile.velocity.X *= 0.99f;
                    Projectile.velocity.Y += 0.3f;

                    if (Projectile.velocity.Y > 16f)
                        Projectile.velocity.Y = 16f;
                }
            }

            int dustType = 171;
            if (Main.rand.NextBool(3))
            {
                dustType = 46;
            }
            if (Main.rand.NextBool(9))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
                Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
                Projectile.rotation += Projectile.spriteDirection * MathHelper.ToRadians(45f);
            }
            //Sticky Behaviour
            Projectile.StickyProjAI(15);
            if (Projectile.ai[0] == 2f)
            {
                Projectile.velocity *= 0f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => Projectile.ModifyHitNPCSticky(20);

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 2f;
            Projectile.timeLeft = 300;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            int dustType = 171;
            if (Main.rand.NextBool(3))
            {
                dustType = 46;
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.Damage();
            for (int i = 0; i < 15; i++)
            {
                int deathDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1.2f);
                Main.dust[deathDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[deathDust].scale = 0.5f;
                    Main.dust[deathDust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 30; j++)
            {
                int deathDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1.7f);
                Main.dust[deathDust2].noGravity = true;
                Main.dust[deathDust2].velocity *= 5f;
                deathDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1f);
                Main.dust[deathDust2].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<SulphuricPoisoning>(), 120);
            target.AddBuff(BuffID.Venom, 60);
        }
    }
}
