using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 27; //60
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ReaverScaleMail>() && legs.type == ModContent.ItemType<ReaverCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CalamityInheritance();
            modPlayer1.reaverMeleeBlast = true;
            player.thorns += 0.33f;
            player.GetAttackSpeed<MeleeDamageClass>() +=0.40f;
            player.GetCritChance<MeleeDamageClass>() += 15;
            player.moveSpeed += 0.20f;
            //25+10近战伤害,15暴击率，30+10近战攻速
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage<MeleeDamageClass>() += 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<PerennialBar>(8)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddIngredient<EssenceofEleum>(2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
