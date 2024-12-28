using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Weapons.Melee;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ExoFlailProj : BaseWhipProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public int Counter = 12;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.extraUpdates = 1;
        }

        public override Color SpecialDrawColor => default;
        public override int ExudeDustType => 269;
        public override int WhipDustType => 107;
        public override int HandleHeight => 56;
        public override int BodyType1StartY => 60;
        public override int BodyType1SectionHeight => 18;
        public override int BodyType2StartY => 86;
        public override int BodyType2SectionHeight => 20;
        public override int TailStartY => 110;
        public override int TailHeight => 50;

        public override void ExtraBehavior()
        {
            Player player = Main.player[Projectile.owner];
            if (Counter > 0)
                Counter--;
            if (Counter <= 0 && Projectile.owner == Main.myPlayer)
            {
                Vector2 vectorBruh = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                if (player.direction != 1)
                {
                    vectorBruh.X = (float)player.bodyFrame.Width - vectorBruh.X;
                }
                if (player.gravDir != 1f)
                {
                    vectorBruh.Y = (float)player.bodyFrame.Height - vectorBruh.Y;
                }
                vectorBruh -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
                Vector2 newCenter = player.RotatedRelativePoint(Main.player[Projectile.owner].position + vectorBruh, true) + Projectile.velocity;
                int moonDamage = (int)(Projectile.damage * 0.5f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), newCenter.X, newCenter.Y, 0f, 0f, ModContent.ProjectileType<ExoFlailEnergy>(), moonDamage, 0f, Projectile.owner, 0f, 0f);
                Counter = 12;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float zero = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity, 20f * Projectile.scale, ref zero))
            {
                return true;
            }
            return false;
        }
        public void Explode(Vector2 Location, float StartAngle, int Streams, float ProjSpeed, int type, float damageMult = 1f)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Streams; i++)
                {
                    Vector2 vector = Utils.RotatedBy(Vector2.Normalize(new Vector2(1f, 1f)), (double)(MathHelper.ToRadians(360 / Streams * i) + StartAngle), default(Vector2));
                    vector.X *= ProjSpeed;
                    vector.Y *= ProjSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Location.X, Location.Y, vector.X, vector.Y, type, (int)((float)Projectile.damage * damageMult), 0, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (Projectile.ai[0] <= 1f && Projectile.localAI[0] == 0f)
            {
                Projectile.ai[0] = 2f;
                float startAngle = MathHelper.ToRadians(Main.rand.Next(3600) / 10);
                if (Projectile.ai[1] >= 11f)
                {
                    Explode(Projectile.Center, startAngle, 8, 12f, ModContent.ProjectileType<ExoFlailEnergy>(), 0.1f);
                    Explode(Projectile.Center, startAngle, 8, 8f, ModContent.ProjectileType<ExoFlailEnergy>(), 0.15f);
                    Explode(Projectile.Center, startAngle, 8, 4f, ModContent.ProjectileType<ExoFlailEnergy>(), 0.2f);
                }
            }
            else
            {
                float startAngle2 = MathHelper.ToRadians(Main.rand.Next(3600) / 10);
                Explode(((Entity)target).Center, startAngle2, 6, 4f, ModContent.ProjectileType<ExoFlailEnergy>(), 0.1f);
                SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
            }
            target.ExoDebuffs();
            Projectile.localAI[1] = 4f;
        }
    }
}
