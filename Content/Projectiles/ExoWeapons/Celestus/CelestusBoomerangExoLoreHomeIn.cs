using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.Celestus
{
    public class CelestusBoomerangExoLoreHomeIn : CIExoProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<Celestusold>();
        public override string Texture => GetInstance<Celestusold>().Texture;
        public float Timer = 0f;
        public int ColorTimer = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
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
            //我在想是不是因为这个无敌帧导致他一个射弹短时间内进行了多判导致c出问题
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.velocity *= -1f;
            Projectile.netUpdate = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.LAP().OnceHitEffect);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.LAP().OnceHitEffect = reader.ReadBoolean();
        }
        //取消返程伤害
        public override bool? CanHitNPC(NPC target)
        {
            return Timer > 60;
        }
        public override void AI()
        {
            if (Projectile.alpha > 50)
                Projectile.alpha -= 10;
            Player player = Main.player[Projectile.owner];
            Timer++;
            if (Timer < 60)
            {
                NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 1500f);
                Vector2 angleToTarget;
                if (target != null)
                    angleToTarget = target.Center;
                else
                    angleToTarget = player.Center;
                //从宙能那抄过来的
                float rot = Projectile.AngleTo(angleToTarget) - MathHelper.PiOver4;
                Projectile.rotation = Projectile.rotation.AngleLerp(rot, 1f);
                Projectile.velocity *= 0.94f;
            }
            else if (Timer >= 60 && Timer < 120)
            {
                Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
                if (Projectile.LAP().OnceHitEffect)
                {
                    Projectile.extraUpdates = 4;
                    Projectile.HomeInNPC(5000f, 30f, 0f);
                }
                else
                    Projectile.velocity *= 0.97f;
            }
            else if(Timer >= 120)
            {
                Projectile.extraUpdates = 3;
                float rot = Projectile.AngleTo(player.Center) - MathHelper.PiOver4;
                Projectile.rotation = Projectile.rotation.AngleLerp(rot, 0.2f);
                float returnSpeed = 25f;
                float acceleration = 5f;
                Projectile.PredictHomeIn(returnSpeed, acceleration, player);
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle projHitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle playerHitbox = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (projHitbox.Intersects(playerHitbox))
                        Projectile.Kill();
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            int red = 135;
            int blue = 206;
            int green = 250;
            int r = 255;
            int g = 255;
            int b = 255;
            ColorTimer++;
            if (r > red) r -= ColorTimer * 5;
            if (b > blue) b -= ColorTimer * 1;
            if (g > green) g -= ColorTimer * 1;
            if (r < red) r = red;
            if (b < blue) b = blue;
            if (g < green) g = green;
            return new Color(r, g, b, Projectile.alpha);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
            Projectile.netSpam = 0;
            SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, Celestusold.getSound) with { MaxInstances = 0 }, Projectile.position);
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
            //将这个生成频率降低了一点，主要是一次弹出的伤害太多了
            if (Projectile.owner == Main.myPlayer)
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 4f;
                double offsetAngle;
                for (int i = 0; i < 2; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 64f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 0.6f), (float)(Math.Cos(offsetAngle) * 0.6f), ProjectileType<Celestus2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 0.6f), (float)(-Math.Cos(offsetAngle) * 0.6f), ProjectileType<Celestus2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
