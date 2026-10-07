using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Core.CINetCode;
using LAP.Core.Utilities;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class StellarContemptProj : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<StellarContempt>();
        public override string Texture => GetInstance<StellarContempt>().Texture;

        private static float RotationIncrement = 0.22f;
        private static int Lifetime = 240;
        private static float ReboundTime = 40f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }

        public override void AI()
        {
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);

            // The hammer makes sound while flying.
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }

            // ai[0]的时候存储锤子正在返回的代码.如果ai[0] = 0f,则代表不在返回.如果ai[0] = 1f, 则代表在返回
            // 等一下...
            // 也就是说我可以在这里改锤子的投掷距离啊
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= ReboundTime)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.tileCollide = false;
                float returnSpeed = 12f;
                float acceleration = 3.2f;
                Player owner = Main.player[Projectile.owner];
                Projectile.PredictHomeIn(returnSpeed, acceleration, owner);
                // Delete the projectile if it touches its owner.
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                    {
                        if (Projectile.CI().Stealth)
                        {
                            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.Owner().GetPlayerToMouseVector2() * 20f, ProjectileType<StellarContemptProj_Rogue>(),
                                Projectile.damage, Projectile.knockBack, Projectile.owner);
                            Main.projectile[p].SetStealthAttack();
                        }
                        Projectile.Kill();
                    }
                }
            }

            // Rotate the hammer as it flies.
            Projectile.rotation += RotationIncrement;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Some dust gets produced on impact.
            int dustCount = Main.rand.Next(20, 24);
            int dustRadius = 6;
            Vector2 corner = new Vector2(target.Center.X - dustRadius, target.Center.Y - dustRadius);
            for (int i = 0; i < dustCount; ++i)
            {
                int dustType = 229;
                float scale = 0.8f + Main.rand.NextFloat(1.1f);
                int idx = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = scale;
            }

            // Applies Nightwither on contact at night.
            target.AddBuff(BuffType<CINightwither>(), 240);

            SpawnFlares(target.Center, target.width, target.height);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Some dust gets produced on impact.
            int dustCount = Main.rand.Next(20, 24);
            int dustRadius = 6;
            Vector2 corner = new Vector2(target.Center.X - dustRadius, target.Center.Y - dustRadius);
            for (int i = 0; i < dustCount; ++i)
            {
                int dustType = 229;
                float scale = 0.8f + Main.rand.NextFloat(1.1f);
                int idx = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = scale;
            }

            // Applies Nightwither on contact at night.
            if (!Main.dayTime)
                target.AddBuff(BuffType<CINightwither>(), 240);

            SpawnFlares(target.Center, target.width, target.height);
        }

        private void SpawnFlares(Vector2 targetPos, int width, int height)
        {
            // Play the Lunar Flare sound centered on the user, not the target (consistent with Lunar Flare and Stellar Striker)
            Player user = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.Item88, Projectile.position);
            Projectile.netUpdate = true;

            int numFlares = 2;
            int flareDamage = (int)(0.3f * Projectile.damage);
            float flareKB = 4f;
            for (int i = 0; i < numFlares; ++i)
            {
                float flareSpeed = Main.rand.NextFloat(8f, 11f);

                // Flares never come from straight up, there is always at least an 80 pixel horizontal offset
                float xDist = Main.rand.NextFloat(80f, 320f) * (Main.rand.NextBool() ? -1f : 1f);
                float yDist = Main.rand.NextFloat(1200f, 1440f);
                Vector2 startPoint = targetPos + new Vector2(xDist, -yDist);

                // The flare is somewhat inaccurate based on the size of the target.
                float xVariance = width / 4f;
                if (xVariance < 8f)
                    xVariance = 8f;
                float yVariance = height / 4f;
                if (yVariance < 8f)
                    yVariance = 8f;
                float xOffset = Main.rand.NextFloat(-xVariance, xVariance);
                float yOffset = Main.rand.NextFloat(-yVariance, yVariance);
                Vector2 offsetTarget = targetPos + new Vector2(xOffset, yOffset);

                // Finalize the velocity vector and make sure it's going at the right speed.
                Vector2 velocity = offsetTarget - startPoint;
                velocity.Normalize();
                velocity *= flareSpeed;

                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPoint, velocity, ProjectileID.LunarFlare, flareDamage, flareKB, Projectile.owner, 0f, targetPos.Y + 300);
                    if (proj.InBounds(Main.maxProjectiles))
                        Main.projectile[proj].DamageType = Projectile.DamageType;
                    Main.projectile[proj].tileCollide = false;
                    ReadSendProjState.SyncedStealth(Main.projectile[proj]);
                }
            }
        }
    }
}
