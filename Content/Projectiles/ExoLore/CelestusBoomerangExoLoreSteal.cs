using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Sounds.Custom;
using CalamityMod.Projectiles.Rogue;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class CelestusBoomerangExoLoreSteal : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/Celestusold";

        private bool initialized = false;
        private float speed = 25f;
        private int counter;
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float TargetIndex => ref Projectile.ai[2];
        public Player Owner => Main.player[Projectile.owner];
        #endregion
        #region 攻击枚举
        const float IsShooted = 0f;
        const float IsReturning = 1f;
        const float IsStealth = 2f;
        #endregion
        #region 射弹属性
        const float ReturnTime = 40f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override void AI()
        {
            DoGeneral();
            switch (AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsReturning:
                    DoReturning(Projectile.Calamity().stealthStrike);
                    break;
                case IsStealth:
                    DoStealth();
                    break;
            }
        }

        private void DoGeneral()
        {
            if (!initialized)
            {
                speed = Projectile.velocity.Length();
                initialized = true;
            }
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
            if (AttackType != IsShooted)
                Projectile.rotation += 1f;
            else
                Projectile.rotation = Projectile.velocity.ToRotation();
            if(AttackType != IsStealth)
                SpawnExtraProj();
        }

        private void SpawnExtraProj()
        {
            counter++;
            if(counter == 12)
            {
                float randomAngle = Main.rand.NextBool() ? MathHelper.PiOver2 + Main.rand.NextFloat(-3f, 4f) : -MathHelper.PiOver2 + Main.rand.NextFloat(-3f, 4f);
                int t = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(randomAngle), ModContent.ProjectileType<CelestusBoomerangExoLoreHomeIn>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                Main.projectile[t].scale *= 0.9f;
                counter = 0;
            }
        }

        private void DoStealth() => CIFunction.HomeInOnNPC(Projectile, true, 1250f, speed, 20f);

        private void DoReturning(bool stealthStrike)
        {
            float returnSpeed = 25f;
            float acceleration = 5f;
            CIFunction.BoomerangReturningAI(Owner, Projectile, returnSpeed, acceleration);
            if (Main.myPlayer != Projectile.owner)
                return;
            if (Projectile.Hitbox.Intersects(Owner.Hitbox))
            {
                if (stealthStrike)
                {
                    Projectile.velocity *= -1f;
                    Projectile.timeLeft = 600;
                    Projectile.penetrate = 1;
                    Projectile.localNPCHitCooldown = -1;
                    AttackType = IsStealth;
                    Projectile.netUpdate = true;
                }
                else
                    Projectile.Kill();

            }
        }

        private void DoShooted()
        {
            AttackTimer += 1f;
            if (AttackTimer > ReturnTime)
            {
                AttackType = IsReturning;
                AttackTimer = 0f;
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
            OnHitEffects();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            OnHitEffects();
        }

        private void OnHitEffects()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                }
            }
            SoundStyle[] getSound =
            [
                CISoundMenu.CelestusOnHit1,
                CISoundMenu.CelestusOnHit2,
                CISoundMenu.CelestusOnHit3
            ];
            SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, getSound), Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Rectangle frame = new Rectangle(0, 0, 106, 94);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Rogue/CelestusoldGlow").Value,
                Projectile.Center - Main.screenPosition,
                frame,
                Color.White,
                Projectile.rotation,
                Projectile.Size / 2,
                1f,
                SpriteEffects.None,
                0);
        }
    }
}
