using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class ExoFlailProj2 : CIBaseWhip
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ExoFlail>();

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.Center - Projectile.Center;
            float hitRange = vector.Length();
            Projectile.rotation = vector.ToRotation();
            if (player.dead || !player.active)
            {
                Projectile.Kill();
                return;
            }
            if (vector.X < 0f)
            {
                player.ChangeDir(1);
                Projectile.direction = 1;
                Projectile.spriteDirection = 1;
                Projectile projectile = Projectile;
                projectile.rotation += (float)Math.PI;
            }
            else
            {
                player.ChangeDir(-1);
                Projectile.direction = -1;
                Projectile.spriteDirection = -1;
            }
            player.itemRotation = (-1f * vector * Projectile.direction).ToRotation();
            player.itemAnimation = 6;
            player.itemTime = 6;
            if (Projectile.ai[0] == 0f && hitRange < ExoFlail.MaxRange && player.whoAmI == Main.myPlayer)
            {
                Vector2 value = player.LocalMouseWorld() - Projectile.Center;
                value.Normalize();
                value.X *= ExoFlail.MouseHomingAcceleration;
                value.Y *= ExoFlail.MouseHomingAcceleration;
                Projectile projectile2 = Projectile;
                projectile2.velocity = projectile2.velocity + value;
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * ExoFlail.Speed;
            }
            if (Projectile.ai[0] == 0f && hitRange > ExoFlail.MaxRange)
            {
                Projectile.ai[0] = 1f;
            }
            if (Projectile.ai[0] != 0f)
            {
                if (hitRange > ExoFlail.MaxRange * 2f)
                {
                    Projectile.Kill();
                    return;
                }
                if (Projectile.ai[0] == 1f)
                {
                    Projectile projectile3 = Projectile;
                    projectile3.velocity = projectile3.velocity + vector.SafeNormalize(Vector2.Zero) * (ExoFlail.ReturnSpeed / 20f);
                }
                if (Projectile.velocity.Length() >= ExoFlail.ReturnSpeed || Projectile.ai[0] == 2f)
                {
                    Projectile.velocity = vector.SafeNormalize(Vector2.Zero) * ExoFlail.ReturnSpeed;
                    Projectile.ai[0] = 2f;
                }
                if (vector.Length() < ExoFlail.ReturnSpeed)
                {
                    Projectile.Kill();
                    return;
                }
            }
            Projectile.ai[1] += 1f;
            Projectile.alpha = Projectile.ai[1] <= 5f ? 255 : 0;
            if (Projectile.ai[1] % 6f == 0f && Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.05f, ProjectileType<ExoFlailEnergy>(), (int)(Projectile.damage * 0.1f), Projectile.owner, 0, 0f);
            }
        }

        public void Explode(Vector2 Location, float StartAngle, int Streams, float ProjSpeed, int type, float damageMult = 1f)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Streams; i++)
                {
                    Vector2 vector = Vector2.Normalize(new Vector2(1f, 1f)).RotatedBy((double)(MathHelper.ToRadians(360 / Streams * i) + StartAngle), default);
                    vector.X *= ProjSpeed;
                    vector.Y *= ProjSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Location.X, Location.Y, vector.X, vector.Y, type, (int)(Projectile.damage * damageMult), 0, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            new Vector2(1.5f, 0f).RotatedBy(Projectile.rotation, default);
            float hitRange = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, player.Center, 22f, ref hitRange))
            {
                Projectile.localAI[0] = 1f;
                return true;
            }
            Projectile.localAI[0] = 0f;
            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.LAP().OnceHitEffect)
            {
                float startAngle = MathHelper.ToRadians(Main.rand.Next(3600) / 10);
                Explode(Projectile.Center, startAngle, 8, 12f, ProjectileType<ExoFlailEnergy>(), 0.1f);
                Explode(Projectile.Center, startAngle, 8, 8f, ProjectileType<ExoFlailEnergy>(), 0.15f);
                Explode(Projectile.Center, startAngle, 8, 4f, ProjectileType<ExoFlailEnergy>(), 0.2f);
                SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
                if (Projectile.ai[2] != 0)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<Exoboomold>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
            else
            {
                float startAngle2 = MathHelper.ToRadians(Main.rand.Next(3600) / 10);
                SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
            }
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            _ = Color.Transparent;
            Texture2D texture = Request<Texture2D>($"{Texture}_Chain").Value;
            Texture2D texture2 = Request<Texture2D>($"{Texture}_Base").Value;
            Vector2 vector = Projectile.Center;
            Rectangle? sourceRectangle = null;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 vector2 = mountedCenter - vector;
            float hitRange = (float)Math.Atan2(vector2.Y, vector2.X) - (float)Math.PI / 2f;
            float num2 = (float)Math.Atan2(vector2.Y, vector2.X) - (float)Math.PI / 2f;
            Vector2 value = new Vector2(-10f, 0f).RotatedBy((double)num2, default);
            if (vector2.X < 0f)
            {
                hitRange += (float)Math.PI;
            }
            bool flag = true;
            if (vector.HasNaNs() || vector2.HasNaNs())
            {
                flag = false;
            }
            while (flag && vector2.Length() >= texture.Height + 1f)
            {
                Vector2 value2 = vector2.SafeNormalize(Vector2.Zero);
                vector += value2 * texture.Height;
                vector2 = mountedCenter - vector;
                Color color = Lighting.GetColor((int)vector.X / 16, (int)vector.Y / 16);
                Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, hitRange, origin, 1f, SpriteEffects.None, 0f);
            }
            Vector2 value3 = vector2;
            value3.Normalize();
            Vector2 value4 = value3 * texture2.Height;
            Color color2 = Lighting.GetColor((int)vector.X / 16, (int)vector.Y / 16);
            Main.spriteBatch.Draw(texture2, mountedCenter - value4 + value - Main.screenPosition, sourceRectangle, color2, num2, origin, 1f, SpriteEffects.None, 0f);
            return true;
        }
    }
}
