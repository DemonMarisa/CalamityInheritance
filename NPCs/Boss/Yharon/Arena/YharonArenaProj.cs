using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Yharon.Arena
{
    public class YharonArenaProj : ModProjectile
    {
        public new string LocalizationCategory => "Boss.Projectiles";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infernado");
            Main.projFrames[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 320;
            Projectile.height = 88;
            Projectile.hostile = true;
            Projectile.scale = 3.3f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360000;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }
        public bool hasSpawn = false;
        public override void AI()
        {
            Projectile.ai[2]++;
            if (!CalamityPlayer.areThereAnyDamnBosses && Projectile.ai[2] > 5)
            {
                Projectile.active = false;
                Projectile.netUpdate = true;
                return;
            }
            
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            // ai0 = 0f的时候才会生成
            if (Projectile.ai[0] == 0f && !hasSpawn)
            {
                for(int i = 0; i < 36; i ++)
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] >= Main.projFrames[Projectile.type])
                        Projectile.ai[1] = 0;

                    var proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - Projectile.height * (i + 1), 0f, 0f, ModContent.ProjectileType<YharonArenaProj>(), 1000, 0, -1, 1);
                    Main.projectile[proj].frame = (int)Projectile.ai[1];
                }
                for (int i = 0; i < 12; i++)
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] >= Main.projFrames[Projectile.type])
                        Projectile.ai[1] = 0;

                    var proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y + Projectile.height * (i + 1), 0f, 0f, ModContent.ProjectileType<YharonArenaProj>(), 1000, 0, -1, 1);
                    Main.projectile[proj].frame = (int)Projectile.ai[1];
                }
                hasSpawn = true;
            }
            CIFunction.BetterAddLight(Projectile.Center, Color.Orange);
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Color.White, Projectile.rotation, new Vector2((float)texture2D13.Width / 2f, (float)num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 600);
        }
    }
}
