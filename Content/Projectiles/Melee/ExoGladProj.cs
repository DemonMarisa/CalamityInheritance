using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ExoGladProj : ModProjectile, ILocalizedModType
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public new string LocalizationCategory => "Content.Projectiles.Melee";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }

        public float counter = 0f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();

            Vector2 value7 = new Vector2(5f, 10f);
            counter += 1f;
            if (counter == 48f)
            {
                counter = 0f;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustType = i == 0 ? 107 : 234;
                    if (Main.rand.NextBool(4))
                    {
                        dustType = CIDustID.DustSandnado;
                    }
                    Vector2 offset = Vector2.UnitX * -12f;
                    offset = -Vector2.UnitY.RotatedBy((double)(counter * 0.1308997f + i * MathHelper.Pi), default) * value7;
                    int exo = Dust.NewDust(Projectile.Center, 0, 0, dustType, 0f, 0f, 160, default, 1.5f);
                    Main.dust[exo].noGravity = true;
                    Main.dust[exo].position = Projectile.Center + offset;
                    Main.dust[exo].velocity = Projectile.velocity;
                    int dusters = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.8f);
                    Main.dust[dusters].noGravity = true;
                    Main.dust[dusters].velocity *= 0f;
                }
            }
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                CalamityUtils.HomeInOnNPC(Projectile, true, 1000f, 22f, 10f);
            }
            else
                CalamityUtils.HomeInOnNPC(Projectile, true, 1000f, 12f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            int dustType = Utils.SelectRandom(Main.rand, new int[]
            {
                107,
                234,
                CIDustID.DustSandnado
            });
            for (int k = 0; k < 4; k++)
            {
                int exo = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, dustType, Projectile.direction * 2, 0f, 150, default, 1f);
                Main.dust[exo].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1f)
            {
                target.ExoDebuffs();
                OnHitEffects(target.Center);
            }
        }
        private void OnHitEffects(Vector2 targetPos)
        {
            Player player = Main.player[Projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();
            var source = Projectile.GetSource_FromThis();
            float swordKB = Projectile.knockBack;
            int swordDmg = (int)(Projectile.damage * 0.25);
            int numSwords = Main.rand.Next(1, 4);
            int spearAmt = Main.rand.Next(1, 4);
            int comet = Main.rand.Next(1, 2);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < numSwords; ++i)
                {
                    CalamityUtils.ProjectileBarrage(source, Projectile.Center, targetPos, Main.rand.NextBool(), 1000f, 1400f, 80f, 900f, Main.rand.NextFloat(24f, 30f), ProjectileType<ExoGladiusBeam>(), swordDmg, swordKB, Projectile.owner);
                }

                for (int n = 0; n < spearAmt; n++)
                {
                    CalamityUtils.ProjectileRain(source, targetPos, 400f, 100f, -1000f, -800f, 29f, ProjectileType<ExoGladSpears>(), swordDmg, swordKB, Projectile.owner);
                }
                if(usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                {

                    ExoGladiusProj.GiveImmue(player, 60, 45);
                    for (int j = 0; j < comet; ++j)
                    {
                        CalamityUtils.ProjectileRain(source, targetPos, 400f, 100f, 500f, 800f, 25f, ProjectileType<ExoGladComet>(), swordDmg, swordKB, Projectile.owner);
                    }

                }
            }
        }
    }
}
