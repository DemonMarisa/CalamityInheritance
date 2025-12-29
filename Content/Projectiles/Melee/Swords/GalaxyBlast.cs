using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class GalaxyBlast : GeneralDamageProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        public override void ExSD()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void AI()
        {
            if (Projectile.ai[1] != -1f && Projectile.position.Y > Projectile.ai[1])
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.position.HasNaNs())
            {
                Projectile.Kill();
                return;
            }
            bool isInTile = WorldGen.SolidTile(Framing.GetTileSafely((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16));
            Dust blastDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f)];
            blastDust.position = Projectile.Center;
            blastDust.velocity = Vector2.Zero;
            blastDust.noGravity = true;
            if (isInTile)
            {
                blastDust.noLight = true;
            }
            if (Projectile.ai[1] == -1f)
            {
                Projectile.ai[0] += 1f;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.penetrate = -1;
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 140;
                Projectile.Center = Projectile.position;
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                if (++Projectile.frameCounter >= Projectile.MaxUpdates * 3)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
                if (Projectile.ai[0] >= (float)(Main.projFrames[Projectile.type] * Projectile.MaxUpdates * 3))
                {
                    Projectile.Kill();
                }
                return;
            }
            Projectile.alpha = 255;
            if (Projectile.numUpdates == 0)
            {
                int npcTracker = -1;
                float homingRange = 60f;
                foreach (NPC nPC in Main.ActiveNPCs)
                {
                    if (nPC.CanBeChasedBy(Projectile, false))
                    {
                        float npcDistance = Projectile.Distance(nPC.Center);
                        if (npcDistance < homingRange && Collision.CanHitLine(Projectile.Center, 0, 0, nPC.Center, 0, 0))
                        {
                            homingRange = npcDistance;
                            npcTracker = nPC.whoAmI;
                        }
                    }
                }
                if (npcTracker != -1)
                {
                    Projectile.ai[0] = 0f;
                    Projectile.ai[1] = -1f;
                    Projectile.netUpdate = true;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            bool insideTile = WorldGen.SolidTile(Framing.GetTileSafely((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16));
            for (int m = 0; m < 4; m++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1.5f);
            }
            for (int n = 0; n < 4; n++)
            {
                int killingPeople = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 2.5f);
                Main.dust[killingPeople].noGravity = true;
                Main.dust[killingPeople].velocity *= 3f;
                if (insideTile)
                {
                    Main.dust[killingPeople].noLight = true;
                }
                killingPeople = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1.5f);
                Main.dust[killingPeople].velocity *= 2f;
                Main.dust[killingPeople].noGravity = true;
                if (insideTile)
                {
                    Main.dust[killingPeople].noLight = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
            Projectile.Kill();
        }
    }
}
