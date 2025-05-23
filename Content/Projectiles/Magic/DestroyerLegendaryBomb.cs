﻿using System;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class DestroyerLegendaryBomb: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public int boomTimer = 120;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            float lights = Main.rand.Next(90, 111) * 0.01f;
            lights *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 1f * lights, 0.2f * lights, 0.75f * lights);
            Projectile.alpha -= 2;
            Projectile.frame = CIFunction.FramesChanger(Projectile, 4, 4);
            Projectile.ai[0] = Main.rand.Next(-100, 101) * 0.0025f;
            Projectile.ai[1] = Main.rand.Next(-100, 101) * 0.0025f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.05f;
                if (Projectile.scale > 1.2)
                {
                    Projectile.localAI[0] = 1f;
                }
            }
            else
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale < 0.8)
                {
                    Projectile.localAI[0] = 0f;
                }
            }
            if (Projectile.ai[2] == -1f)
            {
                CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, Main.DiscoG, 155, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int framing = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = framing * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y6, texture2D13.Width, framing)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, framing / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
               Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<DestroyerLegendaryBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner]; 
            var usPlayer = player.CIMod();
            if (usPlayer.DestroyerTier1)
                modifiers.SetCrit();
            if (usPlayer.DestroyerTier2)
            {
                //取玩家当前法术暴击加成。
                float getCrits = player.GetTotalCritChance<MagicDamageClass>() + 4f;
                if (getCrits > 0f)
                {
                    //将暴击加成小数点前置两位。
                    getCrits /= 100f;
                    //与1结算。
                    float realReduceDefense = 1f - getCrits;
                    if (realReduceDefense < 0f)
                        realReduceDefense = 0f;
                    //直接补到伤害上
                    modifiers.DefenseEffectiveness *= realReduceDefense;
                }
            }
        }
    }
}
