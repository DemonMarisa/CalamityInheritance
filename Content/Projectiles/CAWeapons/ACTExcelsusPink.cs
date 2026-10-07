using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Graphics.DeepGlow;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.CAWeapons
{

    public class ACTExcelsusPink : CIMeleeProj
    {
        public Player Owner => Main.player[Projectile.owner]; public int Timer;
        public float rotSpeed = 0.74f;
        public bool FadeOut;
        public bool FadeOutInit;
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.alpha = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.rotation = Main.rand.NextFloat(6.28f);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(FadeOut);
            writer.Write(FadeOutInit);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            FadeOut = reader.ReadBoolean();
            FadeOutInit = reader.ReadBoolean();
        }
        public override bool? CanDamage() => null;
        public override void AI()
        {
            if (FadeOut)
            {
                if (FadeOutInit)
                {
                    Projectile.timeLeft = 85;
                    FadeOutInit = false;
                }
                Projectile.velocity *= 0.93f;
                rotSpeed *= 0.96f;
                return;
            }
            if (Projectile.timeLeft < 85)
            {
                Projectile.rotation += rotSpeed;
                Projectile.velocity *= 0.93f;
                rotSpeed *= 0.96f;
                return;
            }
            Timer++;
            if (Timer < 18)
            {
                Projectile.rotation += rotSpeed;
                Projectile.velocity *= 0.91f;
                rotSpeed *= 0.96f;
            }
            else if (Timer >= 18 && Timer < 60)
            {
                rotSpeed *= 0.96f;
                Projectile.rotation += rotSpeed;
                Projectile.velocity *= 0.91f;
                rotSpeed *= 0.96f;
                NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 1500);
                if (target is not null)
                {
                    Projectile.rotation = Terraria.Utils.AngleLerp(Projectile.rotation, LAPUtilities.GetVector2(Projectile.Center, target.Center, false).ToRotation(), 0.35f);
                }
            }
            else if (Timer >= 60)
            {
                NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 1500);
                if (target is not null)
                {
                    Projectile.extraUpdates = 2;
                    Projectile.rotation = Terraria.Utils.AngleLerp(Projectile.rotation, LAPUtilities.GetVector2(Projectile.Center, target.Center, false).ToRotation(), 0.35f);
                    Projectile.HomingTarget(target.Center, 1500, 24, 12);
                }
                else
                {
                    rotSpeed *= 0.96f;
                    Projectile.rotation += rotSpeed;
                    Projectile.velocity *= 0.96f;
                    rotSpeed *= 0.96f;
                }
            }


            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return lightColor;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation + MathHelper.PiOver4, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0); 
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Color color;
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                color = new Color(b2, b2, b2, a2);
            }
            else
            {
                color = Color.Violet * 0.75f;
            }

            DeepGlow.SubmitCustomGlow(() =>
            {
                Vector2 origin = new Vector2(39f, 46f);
                Main.EntitySpriteDraw(Request<Texture2D>($"{Texture}Glow").Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation + MathHelper.PiOver4, origin, Projectile.scale, SpriteEffects.None, 0);
            });
            Vector2 origin = new Vector2(39f, 46f);
            Main.EntitySpriteDraw(Request<Texture2D>($"{Texture}Glow").Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation + MathHelper.PiOver4, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Timer > 30)
            {
                FadeOutInit = true;
                FadeOut = true;
            }

            Projectile.netUpdate = true;
            Projectile.netSpam = 0;
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 180);
        }
    }
}
