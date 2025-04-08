﻿using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Skies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.NPCs.Boss.SCAL.ArenaTile
{
    public class LegacyArenaTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            MinPick = int.MaxValue;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(128, 0, 0), CreateMapEntryName());
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                if (!SCalSky.RitualDramaProjectileIsPresent)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                    {
                        WorldGen.KillTile(i, j, false, false, false);
                        if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                    }
                }
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = (float)Main.DiscoR / 255f;
            g = 0f;
            b = 0f;
        }
    }
}
