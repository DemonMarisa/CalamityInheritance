using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Utilities;
using LAP.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class SunSpiritBuff : MinionBuffClass
    {
        public override void UpdateMinion(Player player, CalamityInheritancePlayer usPlayer, ref int buffIndex)
        {
            if (player.HasProj<SunSpiritMinionLegacy>())
            {
                usPlayer.SunSpiritMinionLegacy = true;
            }
            if (!usPlayer.SunSpiritMinionLegacy)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
    public abstract class MinionBuffClass : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public override void Update(Player player, ref int buffIndex)
        {
            UpdateMinion(player, player.CIMod(), ref buffIndex);
        }
        public virtual void UpdateMinion(Player player, CalamityInheritancePlayer usPlayer, ref int buffIndex) { }

    }
}
