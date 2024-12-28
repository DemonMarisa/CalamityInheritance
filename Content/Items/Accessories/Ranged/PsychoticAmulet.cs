using CalamityMod.CalPlayer;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class PsychoticAmulet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            CalamityPlayer modPlayer1 = player.Calamity();
            modPlayer.PsychoticAmulet = true;
            player.shroomiteStealth = true;
            player.GetDamage<ThrowingDamageClass>() += 0.05f;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
            player.GetCritChance(DamageClass.Ranged) += 5;
        }
    }
}
