using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Content.Projectiles;
using CalamityInheritance.Content.Projectiles.Summon;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class ReaverOrbOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.ArmorProj";
        public int dust = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 50;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.alpha = 50;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
        }

        public override void AI()
        {
            bool flag64 = Projectile.type == ModContent.ProjectileType<ReaverOrbOld>();
            Player player = Main.player[Projectile.owner];
            var modPlayer = player.CalamityInheritance();

            if (!modPlayer.reaverSummoner)
            {
                Projectile.active = false;
                return;
            }
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.reaverSummonerOrb = false;
                }
                if (modPlayer.reaverSummonerOrb)
                {
                    Projectile.timeLeft = 2;
                }
            }
            dust--;
            if (dust >= 0)
            {
                int num501 = 50;
                for (int num502 = 0; num502 < num501; num502++)
                {
                    int num503 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, 1f);
                    Main.dust[num503].velocity *= 2f;
                    Main.dust[num503].scale *= 1.15f;
                }
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 1f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width / 2;
            Projectile.position.Y = Main.player[Projectile.owner].Center.Y - Projectile.height / 2 + Main.player[Projectile.owner].gfxOffY - 60f;
            if (Main.player[Projectile.owner].gravDir == -1f)
            {
                Projectile.position.Y = Projectile.position.Y + 120f;
                Projectile.rotation = 3.14f;
            }
            else
            {
                Projectile.rotation = 0f;
            }
            Projectile.position.X = (int)Projectile.position.X;
            Projectile.position.Y = (int)Projectile.position.Y;
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] != 0f)
                {
                    Projectile.ai[0] -= 1f;
                    return;
                }
                bool flag18 = false;
                float num508 = 1200f;
                for (int num512 = 0; num512 < 200; num512++)
                {
                    if (Main.npc[num512].CanBeChasedBy(Projectile, false))
                    {
                        float num513 = Main.npc[num512].position.X + Main.npc[num512].width / 2;
                        float num514 = Main.npc[num512].position.Y + Main.npc[num512].height / 2;
                        float num515 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num513) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num514);
                        if (num515 < num508 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num512].position, Main.npc[num512].width, Main.npc[num512].height))
                        {
                            num508 = num515;
                            flag18 = true;
                        }
                    }
                }
                if (flag18)
                {
                    for (int num252 = 0; num252 < 1; num252++)
                    {
                        int spore = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X - 4f, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y,ModContent.ProjectileType<ReaverOrbMark>(), Projectile.damage, 1.5f, Projectile.owner, 0f, 0f);
                        Main.projectile[spore].minion = true;
                        Main.projectile[spore].minionSlots = 0f;
                        int numberOfProjectiles = Main.rand.Next(6, 8);
                        int[] projectileTypes = { ModContent.ProjectileType<CISporeGasSummon>(), ModContent.ProjectileType<CISporeGasSummon2>(), ModContent.ProjectileType<CISporeGasSummon3>() };
                        float baseAngleIncrement = 2 * MathHelper.Pi / numberOfProjectiles;
                        float randomAngleOffset = (float)(Main.rand.NextDouble() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                        float randomOffset = Main.rand.NextFloat(-MathHelper.ToRadians(2), MathHelper.ToRadians(1));
                        for (int i = 0; i < numberOfProjectiles; i++)
                        {
                            float angle = i * baseAngleIncrement + randomAngleOffset + randomOffset;
                            Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                            int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                            float randomSpeed = Main.rand.NextFloat(2f, 3f);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * randomSpeed, randomProjectileType, Projectile.damage * 1, Projectile.knockBack);
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item77, Projectile.position);
                    Projectile.ai[0] = 50f;
                }
            }
        }

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
        {
            return false;
        }
    }
}
