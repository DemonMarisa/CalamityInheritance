using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using Terraria.UI;
using Terraria.ID;
using CalamityInheritance.Content.Items.Weapons.Melee;

namespace CalamityInheritance.Content.Items
{
    public class CalamityInheritanceGlobalItemLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;
        public override void ModifyItemLoot(Item item, ItemLoot itemloot)
        {
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                itemloot.Add(ModContent.ItemType<Skullmasher>(), 10);
            if (item.type == ModContent.ItemType<AstrumDeusBag>())
                itemloot.Add(ModContent.ItemType<Quasar>(), 10);

            switch (item.type)
            {
                #region Boss Treasure Bags
                case ItemID.MoonLordBossBag:
                    itemloot.Add(ModContent.ItemType<GrandDad>(), 10);
                    break;
                #endregion
            }
        }
    }
}
