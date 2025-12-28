using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.NPCs.Boss.CalamitasClone.LifeSeeker

{
    public class LifeSeekerLegacy : ModNPC
    {
        private int timer = 0;
        private bool start = true;
        public static Asset<Texture2D> GlowTexture;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Life Seeker");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.AsyncLoad);
            }

            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.7f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        #region 图鉴
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				// You can add multiple elements if you really wanted to
				new FlavorTextBestiaryInfoElement($"{GenericNPC.GetNPCBestiaryLocal}.LifeSeekerLegacy")
            ]);
        }
        #endregion
        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 44;
            NPC.height = 30;

            NPC.defense = 10;
            NPC.DR_NERD(0.1f);
            NPC.lifeMax = CalamityWorld.death ? 1500 : 2500;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 15000;
            }

            NPC.aiStyle = -1;
            AIType = -1;

            NPC.knockBackResist = BossRushEvent.BossRushActive ? 0f : 0.25f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            double HPBoost = CalamityServerConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToWater = true;

        }

        public override void AI()
        {
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            if (!NPC.AnyNPCs(ModContent.NPCType<CalamitasCloneLegacy>()))
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                NPC.netUpdate = true;
            }


            NPC parent = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<CalamitasCloneLegacy>())];

            if (start)
            {
                for (int d = 0; d < 15; d++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                }
                NPC.ai[1] = NPC.ai[0];
                start = false;
            }

            NPC.TargetClosest(true);

            float projectileSpeed = 9f;
            Vector2 velocity = Main.player[parent.target].Center - NPC.Center;
            velocity.Normalize();
            velocity *= projectileSpeed;
            NPC.rotation = velocity.ToRotation() + MathHelper.Pi;

            timer++;
            if (timer >= 180)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int d = 0; d < 3; d++)
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);

                    int type = ModContent.ProjectileType<BrimstoneBarrage>();
                    int damage = 300;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, type, damage, 1f, parent.target, 1f, 0f, projectileSpeed * 2f);
                }
                timer = 0;
            }

            double deg = NPC.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = death ? 180 : 150;
            NPC.position.X = parent.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
            NPC.position.Y = parent.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
            NPC.ai[1] += death ? 0.5f : 2f;
        }
        #region 绘制

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

            if (CalamityClientConfig.Instance.Afterimages)
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

            if (CalamityClientConfig.Instance.Afterimages)
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
        #endregion
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
                for (int d = 0; d < 20; d++)
                {
                    int red = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[red].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[red].scale = 0.5f;
                        Main.dust[red].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int d = 0; d < 40; d++)
                {
                    int red = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[red].noGravity = true;
                    Main.dust[red].velocity *= 5f;
                    red = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[red].velocity *= 2f;
                }
            }
        }
    }
}
