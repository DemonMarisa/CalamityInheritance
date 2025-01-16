using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverVisage : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 13; //46
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            modPlayer1.reaverRangedRocket = true;
            //Scarlet:远程伤害降低至30%，暴击概率降低至20%
            player.GetDamage<RangedDamageClass>() += 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage<RangedDamageClass>() += 0.10f;
            player.GetCritChance<RangedDamageClass>() += 10;
            player.ammoCost80 = true;
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<PerennialBar>(),10)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddIngredient(ModContent.ItemType<EssenceofEleum>(), 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
