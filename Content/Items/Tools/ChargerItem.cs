using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Tools
{
    public class ChargerItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.rare = ItemRarityID.Red;

            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.autoReuse = false;
            Item.useTurn = true;
        }

        public override bool? UseItem(Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type > ItemID.Count && player.inventory[i].Calamity().UsesCharge)
                    player.inventory[i].Calamity().Charge = player.inventory[i].Calamity().MaxCharge;
            }
            return true;
        }
    }
}
