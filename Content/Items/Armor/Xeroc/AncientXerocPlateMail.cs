using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientXerocPlateMail : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.GetCritChance<GenericDamageClass>() += 7;
        }
    }
}