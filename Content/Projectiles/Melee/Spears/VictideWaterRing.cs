using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using CalamityMod;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Spears
{
    public class VictideWaterRing : CIMeleeProj
    {
        public bool CanHomeIn => Projectile.ai[0] != 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.scale *= 0.75f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (CanHomeIn)
            {
                if (Projectile.timeLeft > 140)
                    return false;
            }
            else
                return null;
            return null;
        }
        public override void AI()
        {
            if (CanHomeIn)
                Projectile.penetrate = 1;
            else
                Projectile.penetrate = 3;
            Lighting.AddLight(Projectile.Center, 0f, 0f, Projectile.scale * 1.5f);
            int dCounts = 6;
            for (int i = 0; i < dCounts; i++)
            {
                Vector2 dPosition = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dPosition = dPosition.RotatedBy((i - (dCounts / 2 - 1)) * MathHelper.Pi / (double)(float)dCounts, default) + Projectile.Center;
                Vector2 dVelocity = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                int d = Dust.NewDust(dPosition + dVelocity, 0, 0, DustID.DungeonWater, dVelocity.X * 2f, dVelocity.Y * 2f, 100, default, Projectile.scale);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity /= 4f;
                Main.dust[d].velocity -= Projectile.velocity;
            }
            if (CanHomeIn)
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] > 40f)
                {
                    Projectile.tileCollide = true;
                    Projectile.HomeInNPC(600, 12f, 20f);
                }
            }
            //发起追踪前这个玩意无视墙体
            else Projectile.tileCollide = false;
            CIUtils.FramesChanger(Projectile, 4, 3);
            Projectile.rotation += 0.08f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath19, Projectile.Center);
            int dCounts = 36;
            for (int i = 0; i < dCounts; i++)
            {
                Vector2 dPos = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dPos = dPos.RotatedBy((double)((i - (dCounts / 2 - 1)) * MathHelper.TwoPi / dCounts), default) + Projectile.Center;
                Vector2 dVel = dPos - Projectile.Center;
                int d = Dust.NewDust(dPos + dVel, 0, 0, DustID.DungeonWater, dVel.X * 1.5f, dVel.Y * 1.5f, 100, default, 1.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity = dVel / 2;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation ;
            Rectangle rec = texture.Frame(1, 3, 0, Projectile.frame);
            Vector2 rotationPoint = rec.Size() / 2f;
            Main.spriteBatch.Draw(texture, drawPosition, rec, lightColor, drawRotation, rotationPoint, Projectile.scale, 0, 0f);

            return false;
        }
    }
}
