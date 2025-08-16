using CalamityInheritance.Buffs.StatDebuffs;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.NPCProj.Friendly
{
    public class HellblastFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile projectile = Projectile;
            projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
            {
                Projectile projectile2 = Projectile;
                projectile2.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
            Projectile projectile3 = Projectile;
            projectile3.velocity.X = projectile3.velocity.X * 1.04f;
            Projectile projectile4 = Projectile;
            projectile4.velocity.Y = projectile4.velocity.Y * 1.04f;
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + MathHelper.ToRadians(90f);
            CalamityUtils.HomeInOnNPC(Projectile, true, 3000f, 16f, 20f);
            Lighting.AddLight(Projectile.Center, 1f, 0.1f, 0.1f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(214, 94, 106, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 vector = new Vector2((float)TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, (float)Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 position = Projectile.oldPos[i] - Main.screenPosition + vector + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Rectangle(0, Projectile.frame * TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type], TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]), color, Projectile.rotation, vector, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 600, false);
            target.AddBuff(ModContent.BuffType<VulnerabilityHexLegacy>(), 600, false);
            target.AddBuff(ModContent.BuffType<Horror>(), 600, false);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BrimstoneExplosion>(), Projectile.damage / 25, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
