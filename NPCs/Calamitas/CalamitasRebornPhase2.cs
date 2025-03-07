using CalamityInheritance.Content.Items;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Calamitas
{
	[AutoloadBossHead]
    public class CalamitasRebornPhase2: ModNPC
    {
        public static Asset<Texture2D> GlowTexture;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            //不使用任何除了原灾的boss标签以外的封装
            //因此伤害设置上我们不会选择去调用原灾的方法
            NPC.damage = 250; 
            NPC.npcSlots = 14f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.value = CIShopValue.RarityPriceCatalystViolet;
            //进入二阶段移除Boss免伤 -> 作为替代，将二阶段的防御力25 -> 30
            NPC.defense = 30;
			NPC.lifeMax = 150000; //二阶段15万
            if (CalamityConditions.DownedProvidence.IsMet())
            {
                NPC.damage *= 3;
                NPC.defense *= 3;
                NPC.lifeMax *= 3;
                NPC.value *= 2.5f;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            
            //不采用遍历的方法免疫所有的debuff， 只单独对几个特定的debuff免疫打表
            //硫磺火boss当然要免疫硫磺火和地狱火
			NPC.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;
            //近似于机械造物的玩意不太可能会受到毒素的影响
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = true;
            //没了，就免疫上面六个debuff

            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			NPC.chaseable = reader.ReadBoolean();
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
			CalamitasRebornAIPhase2.CalamitasRebornAI(NPC, Mod);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterimageAmt = 7;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    Color afterimageColor = drawColor;
                    afterimageColor = Color.Lerp(afterimageColor, white, colorLerpAmt);
                    afterimageColor = NPC.GetAlpha(afterimageColor);
                    afterimageColor *= (float)(afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
                    offset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, afterimageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 npcOffset = NPC.Center - screenPos;
            npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
            npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, npcOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            texture = GlowTexture.Value;
            Color color = Color.Lerp(Color.White, Color.Red, 0.5f);
            if (Main.zenithWorld)
            {
                color = Color.CornflowerBlue;
            }

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i++)
                {
                    Color extraAfterimageColor = color;
                    extraAfterimageColor = Color.Lerp(extraAfterimageColor, white, colorLerpAmt);
                    extraAfterimageColor *= (float)(afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
                    offset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, extraAfterimageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture, npcOffset, NPC.frame, color, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            return false;
        }
        //DemonMarisa:Onkillɱ��
   //     public override void OnKill()
   //     {
   //         DropHelper.DropBags(NPC);

   //         DropHelper.DropItem(NPC, ItemID.BrokenHeroSword, true);
   //         DropHelper.DropItemChance(NPC, ModContent.ItemType<CalamitasTrophy>(), 10);
   //         DropHelper.DropItemCondition(NPC, ModContent.ItemType<KnowledgeCalamitasClone>(), !CalamityConditions.downedCalamitas);
   //         DropHelper.DropResidentEvilAmmo(NPC, CalamityConditions.downedCalamitas, 4, 2, 1);

			//NPC.Calamity().SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, CalamityConditions.downedCalamitas);

			//if (!Main.expertMode)
   //         {
			//	//Materials
   //             DropHelper.DropItemSpray(NPC, ModContent.ItemType<EssenceofChaos>(), 4, 8);
   //             DropHelper.DropItem(NPC, ModContent.ItemType<CalamityDust>(), 9, 14);
   //             DropHelper.DropItem(NPC, ModContent.ItemType<BlightedLens>(), 1, 2);
			//	DropHelper.DropItemCondition(NPC, ModContent.ItemType<Bloodstone>(), CalamityConditions.downedProvidence, 1f, 30, 40);

   //             // Weapons
   //             DropHelper.DropItemChance(NPC, ModContent.ItemType<TheEyeofCalamitas>(), 4);
   //             DropHelper.DropItemChance(NPC, ModContent.ItemType<Animosity>(), 4);
   //             DropHelper.DropItemChance(NPC, ModContent.ItemType<CalamitasInferno>(), 4);
   //             DropHelper.DropItemChance(NPC, ModContent.ItemType<BlightedEyeStaff>(), 4);

   //             // Equipment
   //             DropHelper.DropItemChance(NPC, ModContent.ItemType<ChaosStone>(), 10);

   //             // Vanity
   //             DropHelper.DropItemChance(NPC, ModContent.ItemType<CalamitasMask>(), 7);
   //         }

   //         // Abyss awakens after killing Calamitas
   //         string key = "Mods.CalamityMod.PlantBossText";
   //         Color messageColor = Color.RoyalBlue;

   //         if (!CalamityConditions.downedCalamitas)
   //         {
   //             if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
   //                 SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/WyrmScream"), (int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y);

   //             if (Main.netMode == NetmodeID.SinglePlayer)
   //                 Main.NewText(Language.GetTextValue(key), messageColor);
   //             else if (Main.netMode == NetmodeID.Server)
   //                 ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
   //         }

   //         // Mark Calamitas as dead
   //         CalamityConditions.downedCalamitas = true;
   //         CalamityMod.UpdateServerBoolean();
   //     }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Calamitas Reborn";
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                //需要石块
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/Calamitas"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/Calamitas2"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/Calamitas3"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/Calamitas4"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/Calamitas5"), 1f);
                //Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CalamitasGores/Calamitas6"), 1f);
                NPC.position.X = NPC.position.X + NPC.width / 2;
                NPC.position.Y = NPC.position.Y + NPC.height / 2;
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - NPC.width / 2;
                NPC.position.Y = NPC.position.Y - NPC.height / 2;
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
           NPC.damage = (int)(NPC.damage * 0.8f);
           NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossAdjustment);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, true);
        }
    }
}
