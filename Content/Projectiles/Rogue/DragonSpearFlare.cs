using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.VanillaArmorChanges;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class DragonSpearFlare: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //只有参考意义, 实际情况下AI会一直试图更新这个东西
            Projectile.timeLeft = 200; 
            Projectile.width = 56;
            Projectile.height = 64;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.localAI[1] > 15f;
        public override void AI()
        {
            //使射弹上升过程中受到重力影响
            if (Main.rand.NextBool(2))
            TrailDust();
            HomingAI();
            
        }
        public void HomingAI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.28f;
            Projectile.velocity.X *= 0.99f;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 30f && Projectile.velocity.Y < 0f)
            {
                Projectile.velocity.Y += 0.42f; 
            }
            Projectile.localAI[1] += 1f; 
            if(Projectile.localAI[1] >= 10f)
            {
                CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
            }
            if (Projectile.localAI[1] < 20f) Projectile.timeLeft = 200;
        }
        public void TrailDust() 
        {
            for (int i = 0; i < 2 ; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, 0f, 0f, 0, default, 0.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 1f;
                d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 0.5f);
                Main.dust[d].velocity *= 1f;
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISoundID.SoundFlamethrower, Projectile.position);
            CIFunction.DustCircle(Projectile.position, 8, 0.5f, CIDustID.DustHeatRay, true, 4f);
        }
    }
}