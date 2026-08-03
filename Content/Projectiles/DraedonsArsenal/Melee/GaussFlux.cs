using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal.Melee
{
    public class GaussFlux : CIMeleeProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public NPC Target
        {
            get => Main.npc[(int)Projectile.ai[1]];
            set => Projectile.ai[1] = value.whoAmI;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Lime.ToVector3());
            if (!Target.active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.Center = Target.Center;
            if (!Main.dedServ)
            {
                if (Time == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Target.Center, DustID.AncientLight);
                        dust.color = Utils.SelectRandom(Main.rand, Color.Yellow, Color.YellowGreen);
                        dust.velocity = Main.rand.NextVector2Circular(20f, 20f);
                        dust.scale = 2f;
                        dust.noGravity = true;
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int arcIndex = 0; arcIndex < 6; arcIndex++)
                    {
                        float offsetAngle = MathHelper.ToRadians(1080f) * i / 18f;
                        offsetAngle += Time / 10f;
                        float scale = 1.4f + (float)Math.Cos(i / 7f * MathHelper.TwoPi + Time / 30f) * 0.3f;
                        scale *= MathHelper.Lerp(1f, 0.4f, arcIndex / 6f);
                        Vector2 offset = Target.Size.RotatedBy(offsetAngle) * 0.5f;
                        offset += (arcIndex * MathHelper.TwoPi / 6f + Time / 20f).ToRotationVector2() * 6f * arcIndex;

                        Dust dust = Dust.NewDustPerfect(Target.Center + offset, DustID.AncientLight);
                        dust.color = Utils.SelectRandom(Main.rand, Color.Yellow, Color.YellowGreen);
                        dust.velocity = Vector2.Zero;
                        dust.scale = scale;
                        dust.noGravity = true;
                    }
                }
            }
            Time++;
        }
    }
}
