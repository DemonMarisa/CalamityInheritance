using CalamityInheritance.Rarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class ScalShopMessage : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 42;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Gray;
        }
    }
}
