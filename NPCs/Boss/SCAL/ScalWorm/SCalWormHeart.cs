using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL.ScalWorm
{
    public class SCalWormHeart : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brimstone Heart");
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 0;
            NPC.LifeMaxNERB(160000, 180000, 90000);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if (CIGlobalNPC.LegacySCal < 0 || !Main.npc[CIGlobalNPC.LegacySCal].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
            NPC.alpha -= 42;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }

            int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
            Main.dust[num622].scale = 0.5f;
            Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                DeathAshParticle.CreateAshesFromNPC(NPC);
                for (int i = 0; i < 5; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dust].scale = 0.5f;
                        Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 5f;
                    dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].velocity *= 2f;
                }
            }
        }
        public override bool PreKill()
        {
            return false;
        }
    }
}
