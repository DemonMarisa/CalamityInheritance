using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class UrsaSergeantLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 26;
            Item.defense = 20;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffType<AstralInfectionDebuff>()] = true;
            player.buffImmune[BuffID.Rabies] = true; //Feral Bite
            //再让我单独往玩家类写字段我不如去自杀
            player.moveSpeed -= 0.15f;
            int actualMaxLife = player.statLifeMax2;
            if (player.statLife <= (int)(actualMaxLife * 0.15))
            {
                player.lifeRegen += 3;
                player.lifeRegenTime += 3;
            }
            else if (player.statLife <= (int)(actualMaxLife * 0.25))
            {
                player.lifeRegen += 2;
                player.lifeRegenTime += 2;
            }
            else if (player.statLife <= (int)(actualMaxLife * 0.5))
            {
                player.lifeRegen += 1;
                player.lifeRegenTime += 1;
            }
        }
    }
}
