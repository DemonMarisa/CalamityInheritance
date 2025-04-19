using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL.Brother
{
    [AutoloadBossHead]
    public class SupremeCataclysmLegacy : ModNPC
    {
        public static readonly SoundStyle BrimstoneFireShotSound = new($"{SupremeCalamitasLegacy.CalScalSoundPath}/BrimstoneFireblastImpact");
        public static readonly SoundStyle BrimstoneSkullSound = new($"{SupremeCalamitasLegacy.CalScalSoundPath}/BrimstoneSkullSound");

        public int distanceY = -375;
        public int distanceX = -750;
        public int projDamage = 200;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.54f,
                Position = new Vector2(0, -10f)
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 100;
			NPC.DR_NERD(0.7f, 0.7f, 0.75f, 0.6f, true);
            NPC.boss = true;

            NPC.LifeMaxNERB(1200000, 1500000);

            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);

            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[ModContent.BuffType<StepToolDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<CryoDrain>()] = false;
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.canGhostHeal = false;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToWater = false;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int associatedNPCType = ModContent.NPCType<SupremeCalamitasLegacy>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement($"{GenericNPC.GetNPCBestiaryLocal}.SupremeCataclysmLegacy")
            });
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
                distanceY += 1;
            }
            else if (NPC.ai[3] < 1500f)
            {
                NPC.ai[3] += 1f;
                distanceY -= 1;
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

            NPC.rotation = MathHelper.PiOver2;

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
                    SoundEngine.PlaySound(BrimstoneSkullSound, NPC.position);
                    NPC.ai[1] = 0f;
                    Vector2 vector85 = new Vector2(NPC.Center.X, NPC.Center.Y);
                    float num689 = 8f;
                    int num691 = ModContent.ProjectileType<BrimstoneWaveLegacy>();
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int num695 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector85.X, vector85.Y, - num689, 0f, num691, projDamage, 0f, Main.myPlayer, 0f, 0f);
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
                    float num689 = 7f;
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
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * num689), (float)(Math.Cos(offsetAngle) * num689), ModContent.ProjectileType<BrimstoneBarrageLegacy>(), projDamage, 0f, Main.myPlayer, 0f, 1f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * num689), (float)(-Math.Cos(offsetAngle) * num689), ModContent.ProjectileType<BrimstoneBarrageLegacy>(), projDamage, 0f, Main.myPlayer, 0f, 1f);
                        }
                    }
                    for (int dust = 0; dust <= 5; dust++)
                    {
                        Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f);
                    }
                }
            }
            for (int num621 = 0; num621 < 2; num621++)
            {
                int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                Main.dust[num622].scale = 0.5f;
                Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
            }
        }
        /*
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            return !CalamityUtils.AntiButcher(NPC, ref damage, 0.5f);
        }
        */
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
            if (CIGlobalNPC.LegacySCalLament != -1)
                texture2D15 = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Brother/SupremeCataclysmLegacy_Blue").Value;

            Vector2 vector11 = new(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 7;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture2D15.Width, (texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, (texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Brother/SupremeCataclysmLegacyGlow").Value;
            if (CIGlobalNPC.LegacySCalLament != -1)
                texture2D15 = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Brother/SupremeCataclysmLegacyGlow_BLue").Value;

            Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 *= (num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2(texture2D15.Width, (texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

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
            for (int num623 = 0; num623 < 2; num623++)
            {
                int num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, CIDustID.DustMushroomSpray113, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, CIDustID.DustMushroomSpray113, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
        }
        public override void OnKill()
        {
            DeathAshParticle.CreateAshesFromNPC(NPC);
            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/BrothersDeath2") with { Pitch = -0.65f, Volume = 1.8f }, NPC.Center);
            for (int num621 = 0; num621 < 40; num621++)
            {
                int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, CIDustID.DustMushroomSpray113, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 40; num623++)
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
