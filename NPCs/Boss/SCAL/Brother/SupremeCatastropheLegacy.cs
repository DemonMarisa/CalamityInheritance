using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Boss;
using CalamityInheritance.NPCs.Boss.SCAL.Brother;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.NPCs.Boss.SCAL.Brother
{
    [AutoloadBossHead]
    public class SupremeCatastropheLegacy : ModNPC
    {
        public static readonly SoundStyle BrimstoneShotSound = new($"{SupremeCalamitasLegacy.CalScalSoundPath}/BrimstoneHellblastSound");
        public static readonly SoundStyle BrimstoneFireShotSound = new($"{SupremeCalamitasLegacy.CalScalSoundPath}/BrimstoneFireblastImpact");

        public int distanceY = 375;
        public int distanceX = 750;
        public int projDamage = 200;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Catastrophe");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.npcSlots = 5f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 100;
			NPC.DR_NERD(0.7f, 0.7f, 0.75f, 0.6f, true);

			NPC.LifeMaxNERB(1200000, 1500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.canGhostHeal = false;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToWater = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(distanceY);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            distanceY = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            CIGlobalNPC.LegacySCalCatastrophe = NPC.whoAmI;

            if (CIGlobalNPC.LegacySCal < 0 || !Main.npc[CIGlobalNPC.LegacySCal].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];
            float acceleration = 1.5f;

            // ���ս����
            Item targetSelectedItem = target.inventory[target.selectedItem];
            if (targetSelectedItem.CountsAsClass(ModContent.GetInstance<TrueMeleeDamageClass>()) || targetSelectedItem.CountsAsClass(ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>()))
                acceleration *= 0.5f;

            #region Y�ƶ�
            if (NPC.ai[3] < 750f)
            {
                NPC.ai[3] += 1f;
                distanceY -= 1;
            }
            else if (NPC.ai[3] < 1500f)
            {
                NPC.ai[3] += 1f;
                distanceY += 1;
            }
            if (NPC.ai[3] >= 1500f)
            {
                NPC.ai[3] = 0f;
            }
            #endregion

            #region �ƶ�
            // ˵ʵ����Ҳ��֪��Ϊʲô�����ǲ���ôд��ԭ������û��ζ
            Vector2 npcCenter = NPC.Center;
            Vector2 targetPos = new Vector2(target.Center.X - distanceX, target.Center.Y + distanceY);
            Vector2 direction = targetPos - npcCenter;
            direction.SafeNormalize(Vector2.Zero);

            NPC.rotation = -MathHelper.PiOver2;

            if (NPC.velocity.X < direction.X)
            {
                NPC.velocity.X = NPC.velocity.X + acceleration;
                if (NPC.velocity.X < 0f && direction.X > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X + acceleration;
                }
            }
            else if (NPC.velocity.X > direction.X)
            {
                NPC.velocity.X = NPC.velocity.X - acceleration;
                if (NPC.velocity.X > 0f && direction.X < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X - acceleration;
                }
            }
            if (NPC.velocity.Y < direction.Y)
            {
                NPC.velocity.Y = NPC.velocity.Y + acceleration;
                if (NPC.velocity.Y < 0f && direction.Y > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + acceleration;
                }
            }
            else if (NPC.velocity.Y > direction.Y)
            {
                NPC.velocity.Y = NPC.velocity.Y - acceleration;
                if (NPC.velocity.Y > 0f && direction.Y < 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - acceleration;
                }
            }
            #endregion

            if (NPC.localAI[0] < 120f)
            {
                NPC.localAI[0] += 1f;
            }
            if (NPC.localAI[0] >= 120f)
            {
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 30f)
                {
                    SoundEngine.PlaySound(BrimstoneShotSound, NPC.position);
                    NPC.ai[1] = 0f;
                    Vector2 vector85 = new Vector2(NPC.Center.X, NPC.Center.Y);
                    float speedX = 4f;
                    int dType = ModContent.ProjectileType<BrimstoneHellblast2>();
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int num695 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector85.X, vector85.Y, speedX, 0f, dType, projDamage, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
                NPC.ai[2] += 1f;
                if (!NPC.AnyNPCs(ModContent.NPCType<SupremeCataclysmLegacy>()))
                {
                    NPC.ai[2] += 2f;
                }
                if (NPC.ai[2] >= 300f)
                {
                    NPC.ai[2] = 0f;
                    float speedX = 7f;
                    SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.position);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(NPC.velocity.X, NPC.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * speedX), (float)(Math.Cos(offsetAngle) * speedX), ModContent.ProjectileType<BrimstoneBarrageLegacy>(), projDamage, 0f, Main.myPlayer, 0f, 1f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * speedX), (float)(-Math.Cos(offsetAngle) * speedX), ModContent.ProjectileType<BrimstoneBarrageLegacy>(), projDamage, 0f, Main.myPlayer, 0f, 1f);
                        }
                    }
                    for (int dust = 0; dust <= 5; dust++)
                    {
                        Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f);
                    }
                }
            }
            for (int j = 0; j < 2; j++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                Main.dust[d].scale = 0.5f;
                Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D npcTex = TextureAssets.Npc[NPC.type].Value;
			Vector2 origiVel = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color white = Color.White;
			float cLerp = 0.5f;
			int afterAmt = 7;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int i = 1; i < afterAmt; i += 2)
				{
					Color cOrigi = drawColor;
					cOrigi = Color.Lerp(cOrigi, white, cLerp);
					cOrigi = NPC.GetAlpha(cOrigi);
					cOrigi *= (afterAmt - i) / 15f;
					Vector2 drawVel = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					drawVel -= new Vector2(npcTex.Width, npcTex.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					drawVel += origiVel * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(npcTex, drawVel, NPC.frame, cOrigi, NPC.rotation, origiVel, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 oriVelEx = NPC.Center - Main.screenPosition;
			oriVelEx -= new Vector2(npcTex.Width, npcTex.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			oriVelEx += origiVel * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(npcTex, oriVelEx, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origiVel, NPC.scale, spriteEffects, 0f);

			npcTex = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Brother/SupremeCatastropheLegacyGlow").Value;
            Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int j = 1; j < afterAmt; j++)
				{
					Color exColor = color37;
					exColor = Color.Lerp(exColor, white, cLerp);
					exColor *= (afterAmt - j) / 15f;
					Vector2 vector44 = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2(npcTex.Width, npcTex.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector44 += origiVel * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(npcTex, vector44, NPC.frame, exColor, NPC.rotation, origiVel, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(npcTex, oriVelEx, NPC.frame, color37, NPC.rotation, origiVel, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CheckActive()
        {
            return false;
        }
        /*
        public override bool PreKill()
        {
            return false;
        }
        */
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                int d2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                d2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                Main.dust[d2].velocity *= 2f;
            }
        }
        public override void OnKill()
        {
            DeathAshParticle.CreateAshesFromNPC(NPC);
            SoundEngine.PlaySound(new SoundStyle($"{SupremeCalamitasLegacy.CalScalSoundPath}/BrothersDeath1") with { Pitch = -0.65f, Volume = 1.8f }, NPC.Center);
            for (int j = 0; j < 40; j++)
            {
                int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, CIDustID.DustMushroomSpray113, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 40; i++)
            {
                int num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, CIDustID.DustMushroomSpray113, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, CIDustID.DustMushroomSpray113, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
        }
    }
}
