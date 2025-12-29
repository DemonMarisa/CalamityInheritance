using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class ExorcismProjLegacy : RogueDamageProj
    {
        public override string Texture => GetInstance<ExorcismLegacy>().Texture;
        public override void ExSD()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation += 0.05f * Projectile.direction;

            // Dust Effects
            Vector2 dustLeft = (new Vector2(-1, 0)).RotatedBy(Projectile.rotation);
            Vector2 dustRight = (new Vector2(1, 0)).RotatedBy(Projectile.rotation);
            Vector2 dustUp = (new Vector2(0, -1)).RotatedBy(Projectile.rotation);
            Vector2 dustDown = (new Vector2(0, 1) * 2f).RotatedBy(Projectile.rotation);

            float minSpeed = 1.5f;
            float maxSpeed = 5f;
            float minScale = 0.8f;
            float maxScale = 1.4f;

            int dustType = 175;
            int dustCount = (int)(5 * (Projectile.ai[0]));

            for (int i = 0; i < dustCount; i++)
            {
                Dust left = Dust.NewDustPerfect(Projectile.Center, dustType);
                left.noGravity = true;
                left.position = Projectile.Center;
                left.velocity = dustLeft * Main.rand.NextFloat(minSpeed, maxSpeed) + Projectile.velocity;
                left.scale = Main.rand.NextFloat(minScale, maxScale);

                Dust right = Dust.NewDustPerfect(Projectile.Center, dustType);
                right.noGravity = true;
                right.position = Projectile.Center;
                right.velocity = dustRight * Main.rand.NextFloat(minSpeed, maxSpeed) + Projectile.velocity;
                right.scale = Main.rand.NextFloat(minScale, maxScale);

                Dust up = Dust.NewDustPerfect(Projectile.Center, dustType);
                up.noGravity = true;
                up.position = Projectile.Center;
                up.velocity = dustUp * Main.rand.NextFloat(minSpeed, maxSpeed) + Projectile.velocity;
                up.scale = Main.rand.NextFloat(minScale, maxScale);
                 
                Dust down = Dust.NewDustPerfect(Projectile.Center, dustType);
                down.noGravity = true;
                down.position = Projectile.Center;
                down.velocity = dustDown * Main.rand.NextFloat(minSpeed, maxSpeed) + Projectile.velocity;
                down.scale = Main.rand.NextFloat(minScale, maxScale);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Stars
            if (IsStealth)
                return;
            for (int i = 0; i < 6; i++)
            {
                Vector2 pos = new Vector2(Projectile.Center.X + (float)Projectile.width * 0.5f + (float)Main.rand.Next(-201, 201), Main.screenPosition.Y - 600f - Main.rand.Next(50));
                Vector2 starVelocity = CalamityUtils.CalculatePredictiveAimToTargetMaxUpdates(pos, target, 30f, 2);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, starVelocity, ModContent.ProjectileType<ExorcismStarLegacy>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack * 0.5f, Projectile.owner, Main.rand.NextFloat(-3f, 3f), 0f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.GetTexture();
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Projectile.Kill();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            //Crystal smash sound
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            // Light burst
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ExorcismShockwave>(), (int)(Projectile.damage * 0.8f), 0, Projectile.owner, Projectile.ai[0], 0);
            Main.projectile[p].rotation = Projectile.rotation;
            Main.projectile[p].Calamity().stealthStrike = IsStealth;
        }
    }
}
