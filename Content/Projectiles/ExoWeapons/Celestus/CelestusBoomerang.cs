using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Content.Misc;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.Celestus
{
    public class CelestusBoomerang : CIExoProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<Celestusold>();
        public override string Texture => GetInstance<Celestusold>().Texture;

        public bool initialized = false;
        public bool exoLore;
        public float speed = 25f;
        public int counter;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!initialized)
            {
                speed = Projectile.velocity.Length();
                initialized = true;
            }

            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
            Projectile.rotation += 1f;

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }

            switch (Projectile.ai[0])
            {
                case 0f:
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] >= 40f)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.ai[1] = 0f;
                        Projectile.netUpdate = true;
                    }
                    if (exoLore && Projectile.CI().Stealth)
                    {
                        counter++;
                        if (counter == 12)
                        {
                            float randomAngle = Main.rand.NextBool() ? MathHelper.PiOver2 + Main.rand.NextFloat(-3f, 4f) : -MathHelper.PiOver2 + Main.rand.NextFloat(-3f, 4f);
                            int t = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(randomAngle), ProjectileType<CelestusBoomerangExoLoreHomeIn>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                            Main.projectile[t].scale *= 0.9f;
                            counter = 0;
                        }
                    }
                    break;
                case 1f:
                    float returnSpeed = 25f;
                    float acceleration = 5f;
                    Projectile.PredictHomeIn(returnSpeed, acceleration, Projectile.Owner());
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Rectangle projHitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                        Rectangle playerHitbox = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                        if (projHitbox.Intersects(playerHitbox))
                        {
                            if (Projectile.CI().Stealth)
                            {
                                Projectile.velocity *= -1f;
                                Projectile.timeLeft = 600;
                                Projectile.penetrate = 1;
                                Projectile.localNPCHitCooldown = -1;
                                Projectile.ai[0] = 2f;
                                Projectile.netUpdate = true;
                            }
                            else
                                Projectile.Kill();
                        }
                    }
                    break;
                case 2f:
                    Projectile.HomeInNPC(1250f, speed * 2f, 20f);
                    break;
                default:
                    break;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
            OnHitEffects();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ProjectileType<Celestus2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ProjectileType<Celestus2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                }
            }
            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Rectangle frame = new Rectangle(0, 0, 106, 94);
            Main.EntitySpriteDraw(Request<Texture2D>($"{GetInstance<Celestusold>().Texture}Glow").Value,
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
