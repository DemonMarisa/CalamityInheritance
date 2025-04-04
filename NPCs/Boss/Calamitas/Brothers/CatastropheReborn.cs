using System.IO;
using CalamityInheritance.NPCs.Boss.Calamitas;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.NPCs.Boss.Calamitas.Brothers

{
    [AutoloadBossHead]
    public class CatastropheReborn : ModNPC
    {
        public static Asset<Texture2D> GlowMask;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type); //录入图鉴
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() //图鉴绘制
            {
                PortraitScale = 0.8f, //预览图大小
                Scale = 0.5f, //图集大小
                PortraitPositionYOverride = 0
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            if (!Main.dedServ)
            {
                GlowMask = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad); //寻找匹配的glow
            }
        }
        public override void SetDefaults()
        {
            NPC.BossBar = Main.BigBossProgressBar.NeverValid; //设置boss血条
            NPC.Calamity().canBreakPlayerDefense = false; //取消防损
            NPC.damage = 200; //接触伤害200, 后面需要用这个伤害去算弹幕的伤害
            NPC.npcSlots = 5f; //需要占用的npc栏位
            NPC.width = NPC.height = 120;
            NPC.lifeMax = 50000;
            NPC.scale *= 1.35f;
            NPC.defense = 20; //死亡模式下20点防御
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //写入图鉴
            int calNPC = ModContent.NPCType<CalamitasReborn>(); //有一说一我图鉴百八十年都没开过一回，我也不知道图鉴这么写会变成什么样
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[calNPC], quickUnlock: true);

            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                // new FlavorTextBestiaryInfoElement("Mods.CalamityInheritance.Bestiary.CataclysmReborn") //get图鉴的文本
            ]);
        }

        public override void SendExtraAI(BinaryWriter writer)
        //只有你需要除了NPC.ai[]的数组以外的数组去存放AI的时候，才会用这个函数
        //恰好，我们就需要这种东西
        {
            for (int i = 0; i < 4; i++)
            {
                writer.Write(NPC.CIMod().BossNewAI[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        //接收写入的额外AI.
        //一定是与上面的SendExtra一起进行的
        {
            for (int i = 0; i < 4; i++)
                NPC.CIMod().BossNewAI[i] = reader.ReadSingle();
        }

        public override void FindFrame(int frameHeight)
        {
            //绘制npc的动画
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            CatastropheAI.ThisAI(NPC, Mod);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //绘制残影
            SpriteEffects sEffect = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                sEffect = SpriteEffects.FlipHorizontally;

            Texture2D getTexture = TextureAssets.Npc[NPC.type].Value;
            Vector2 halveTexture = new(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            int afterAmt = 7;
            for (int i = 1; i < afterAmt; i += 2)
            {
                Color afterColor = drawColor; afterColor = Color.Lerp(afterColor, Color.Wheat, 0.5f);
                afterColor = NPC.GetAlpha(afterColor);
                afterColor *= (afterAmt - i) / 15f;
                Vector2 afterDrawCenter = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                afterDrawCenter -= new Vector2(getTexture.Width, getTexture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                afterDrawCenter += halveTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(getTexture, afterDrawCenter, NPC.frame, afterColor, NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);
            }
            Vector2 drawLocation = NPC.Center - screenPos;
            drawLocation -= new Vector2(getTexture.Width, getTexture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            drawLocation += halveTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(getTexture, drawLocation, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);

            getTexture = GlowMask.Value;
            Color pink = Color.Lerp(Color.White, Color.Red, 0.5f);
            for (int j = 1; j < afterAmt; j++)
            {
                Color afterColorNew = pink;
                afterColorNew = Color.Lerp(afterColorNew, Color.Wheat, 0.5f);
                afterColorNew *= (afterAmt - j) / 15f;
                Vector2 afterDrawCenterNew = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                afterDrawCenterNew -= new Vector2(getTexture.Width, getTexture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                afterDrawCenterNew += halveTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(getTexture, afterDrawCenterNew, NPC.frame, afterColorNew, NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);
            }
            spriteBatch.Draw(getTexture, drawLocation, NPC.frame, pink, NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);
            return false;
        }
        public override bool CheckActive() => false;

        // public override void OnKill() 什么？你不会以为我会允许兄弟被打掉的时候产红心吧？
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f);
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    //这里需要一大堆石块
                }
                NPC.position.X = NPC.position.X + NPC.width / 2;
                NPC.position.Y = NPC.position.Y + NPC.height / 2;
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - NPC.width / 2;
                NPC.position.Y = NPC.position.Y - NPC.height / 2;
                for (int j = 0; j < 40; j++)
                {
                    int dType = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dType].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[dType].scale = 0.5f;
                        Main.dust[dType].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int k = 0; k < 70; k++)
                {
                    int dType2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[dType2].velocity *= 5f;
                    Main.dust[dType2].noGravity = true;
                    dType2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dType2].velocity *= 2f;
                }
            }

        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120, true);
        }
    }
}
