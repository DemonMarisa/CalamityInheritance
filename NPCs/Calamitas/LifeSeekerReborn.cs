using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.NPCs.Calamitas

{
    public class LifeSeekerReborn : ModNPC
    {
        public static Asset<Texture2D> GlowTexture;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Life Seeker");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.damage = 30;
            NPC.width = 44;
            NPC.height = 30;
            NPC.defense = 8;
            NPC.lifeMax = 200;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 30000;
            }
            NPC.aiStyle = 5;
            AIType = NPCID.Probe;
            NPC.knockBackResist = BossRushEvent.BossRushActive ? 0f : 0.25f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.buffImmune[BuffID.OnFire] = true;
        }

		public override bool PreAI()
		{
			// Setting this in SetDefaults will disable expert mode scaling, so put it here instead
			NPC.damage = 0;

			return true;
		}

		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / Main.npcFrameCount[NPC.type] / 2f);
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterImageAmt = 5;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int a = 1; a < afterImageAmt; a += 2)
                {
                    Color afterImageColor = drawColor;
                    afterImageColor = Color.Lerp(afterImageColor, white, colorLerpAmt);
                    afterImageColor = NPC.GetAlpha(afterImageColor);
                    afterImageColor *= (afterImageAmt - a) / 15f;
                    Vector2 afterimagePos = NPC.oldPos[a] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    afterimagePos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    afterimagePos += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, afterimagePos, NPC.frame, afterImageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 drawPos = NPC.Center - screenPos;
            drawPos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            drawPos += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            texture = GlowTexture.Value;
            Color glow = Color.Lerp(Color.White, Color.Red, colorLerpAmt);

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int a = 1; a < afterImageAmt; a++)
                {
                    Color glowColor = glow;
                    glowColor = Color.Lerp(glowColor, white, colorLerpAmt);
                    glowColor *= (afterImageAmt - a) / 15f;
                    Vector2 afterimagePos = NPC.oldPos[a] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    afterimagePos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    afterimagePos += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, afterimagePos, NPC.frame, glowColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture, drawPos, NPC.frame, white, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
                }
                //��Ȼ��ʱ����
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/LifeSeeker"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/LifeSeeker2"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/LifeSeeker3"), 1f);
            }
        }
    }
}
