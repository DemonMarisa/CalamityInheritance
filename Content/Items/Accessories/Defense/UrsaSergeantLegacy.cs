using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class UrsaSergeantLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
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
            player.buffImmune[BuffType<CIAstralInfection>()] = true;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.AstralInfectionDebuff] = true;
            player.buffImmune[BuffID.Rabies] = true; //Feral Bite
            //再让我单独往玩家类写字段我不如去自杀
            player.moveSpeed -= 0.15f;
            int actualMaxLife = player.statLifeMax2;
            if (player.statLife <= (int)(actualMaxLife * 0.15))
            {
                player.LAP().LifeRegen += 3;
                player.lifeRegenTime += 3;
            }
            else if (player.statLife <= (int)(actualMaxLife * 0.25))
            {
                player.LAP().LifeRegen += 2;
                player.lifeRegenTime += 2;
            }
            else if (player.statLife <= (int)(actualMaxLife * 0.5))
            {
                player.LAP().LifeRegen += 1;
                player.lifeRegenTime += 1;
            }
        }
    }
}
