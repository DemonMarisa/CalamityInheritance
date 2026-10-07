using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Core.CINetCode;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class StellarContemptMountProj_Rogue : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<StellarContempt>();
        public override string Texture => GetInstance<StellarContempt>().Texture;
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.35f };
        public int Timer;
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.timeLeft = 360;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;
            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);
            //锤子飞行过程中应当有声音
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            Timer++;
            float pVelAcceleration = 0.197f;
            if (Timer < 15)
            {
                pVelAcceleration = 0.017f;
                Projectile.velocity.X -= 0.001f;
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.velocity.X *= 0.997f;
            Projectile.velocity.Y += pVelAcceleration;
            if (Timer > 65)
                Projectile.HomeInNPC(1900f, 27f, 24f, 20f);
            else
            {
                Projectile.timeLeft = 360;
            }
            Projectile.rotation += 0.2f;
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.4f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.4f + velOffset.Y;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.Vortex, new Vector2(dFlyVelX, dFlyVelY), 100, default, 1f);
                dust.noGravity = true;
            }
            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.5f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.5f + velOffset.Y;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.Electric, new Vector2(dFlyVelX, dFlyVelY), 100, default, 1f);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
            //挂载每次击中都会提高攻击频率
            if (Projectile.localNPCHitCooldown > 3)
                Projectile.localNPCHitCooldown -= 3;
            if (Projectile.localNPCHitCooldown < 3)
                Projectile.localNPCHitCooldown = 3;
            //从灾厄上抄下来的, 由于有一些特殊效果所以粒子会少一点
            float numberOfDusts = 15f;
            float rotFactor = 360f / numberOfDusts;
            for (int i = 0; i < numberOfDusts; i++)
            {
                float rot = MathHelper.ToRadians(i * rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Vortex, new Vector2(velOffset.X, velOffset.Y), 0, default, 0.5f);
                dust.noGravity = true;
                dust.velocity = velOffset;
                dust.scale = Main.rand.NextFloat(1.5f, 3.2f);
            }

            // 固定造成Nihtwither的debuff
            target.AddBuff(BuffType<CINightwither>(), 240);
            SpawnFlares(target.Center, target.width, target.height);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int dustCount = Main.rand.Next(20, 24);
            int dustRadius = 6;
            Vector2 corner = new(target.Center.X - dustRadius, target.Center.Y - dustRadius);
            for (int i = 0; i < dustCount; ++i)
            {
                int dustType = 229;
                float scale = 0.8f + Main.rand.NextFloat(1.1f);
                int idx = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = scale;
            }

            if (!Main.dayTime)
                target.AddBuff(BuffType<CINightwither>(), 240);

            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);

            SpawnFlares(target.Center, target.width, target.height);
        }

        private void SpawnFlares(Vector2 targetPos, int width, int height)
        {
            int numFlares = 3;
            int flareDamage = (int)(0.2f * Projectile.damage); //伤害跑高
            float flareKB = 4f;
            for (int i = 0; i < numFlares; ++i)
            {
                float flareSpeed = Main.rand.NextFloat(9f, 13f);

                float xDist = Main.rand.NextFloat(80f, 320f) * (Main.rand.NextBool() ? -1f : 1f);
                float yDist = Main.rand.NextFloat(1200f, 1440f);
                Vector2 startPoint = targetPos + new Vector2(xDist, -yDist);

                float xVariance = width / 4f;
                if (xVariance < 8f)
                    xVariance = 8f;
                float yVariance = height / 4f;
                if (yVariance < 8f)
                    yVariance = 8f;
                float xOffset = Main.rand.NextFloat(-xVariance, xVariance);
                float yOffset = Main.rand.NextFloat(-yVariance, yVariance);
                Vector2 offsetTarget = targetPos + new Vector2(xOffset, yOffset);

                Vector2 velocity = offsetTarget - startPoint;
                velocity.Normalize();
                velocity *= flareSpeed;
                //由于锤子会在敌怪身上旋转一段时间造成持续性的伤害, 因此这里生成的月耀射弹将会是取1/2的概率
                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPoint, velocity, ProjectileID.LunarFlare, flareDamage, flareKB, Main.myPlayer, 0f, targetPos.Y + 300);
                    Main.projectile[proj].DamageType = Projectile.DamageType;
                    Main.projectile[proj].tileCollide = false;
                    ReadSendProjState.SyncedStealth(Main.projectile[proj]);
                }
            }
        }
    }
}
