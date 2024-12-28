using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.World;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Buffs.Potions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class PurifiedJam : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            if (!CalamityWorld.death)
            {
                return;
            }
            foreach (TooltipLine item in list)
            {
                if (item.Mod == "Terraria" && item.Name == "Tooltip0")
                {
                    item.Text = (string)CalamityUtils.GetText("Mods.CalamityInheritance.Item.Tooltip.PurifiedJam");
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.FindBuffIndex(BuffID.PotionSickness) == -1;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Invincible>(), (CalamityWorld.death) ? 300 : 600);
            player.AddBuff(BuffID.PotionSickness, player.pStone ? 1500 : 1800);
            return true;
        }
    }
}
