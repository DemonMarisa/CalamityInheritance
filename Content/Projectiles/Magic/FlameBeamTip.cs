using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class FlameBeamTip : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 4;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override bool PreAI()
        {
            if (Projectile.ai[0] != 0f)
                if (Projectile.alpha < 170 && Projectile.alpha + 5 >= 170)
                    Projectile.alpha += 5;

            return true;
        }
    
        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.alpha -= 50;
                if (Projectile.alpha <= 0)
                {
                    Projectile.alpha = 0;
                    Projectile.ai[0] = 1f;
                    if (Projectile.ai[1] == 0f)
                    {
                        Projectile.ai[1] += 1f;
                        Projectile.position += Projectile.velocity;
                    }
                    if (Main.myPlayer == Projectile.owner)
                    {
                        int getProj = Projectile.type;
                        if (Projectile.ai[1] >= 15 + Main.rand.Next(3))
                        {
                            getProj = ProjectileType<FlameBeamTip2>();
                        }

                        int whatProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + Projectile.velocity.X + Projectile.width / 2, Projectile.position.Y + Projectile.velocity.Y + Projectile.height / 2,
                            Projectile.velocity.X, Projectile.velocity.Y, getProj, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, Projectile.ai[1] + 1f);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, whatProj, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            else
            {
                if (Projectile.alpha == 150)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.01f, Projectile.velocity.Y * 0.01f, 200, default, 2f);
                        Main.dust[d].noGravity = true;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
            target.AddBuff(BuffID.OnFire3, 300);
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity * 0f, ProjectileType<FuckYou>(), Projectile.damage/2, 0f , Projectile.owner);
            Main.projectile[proj].DamageType = DamageClass.Magic;
        }
    }
}
