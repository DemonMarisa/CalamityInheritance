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
            player.GetAttackSpeed<MeleeDamageClass>() +=0.30f;
            player.GetCritChance<MeleeDamageClass>() += 5;
            player.moveSpeed += 0.20f;
            //Scarlet:近战暴击概率下调至10%，常驻的总伤害下调至20%，但是常驻攻速上升至40%
            //DemonMarisa:改了
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.moveSpeed += 0.1f;
            player.GetDamage<MeleeDamageClass>() += 0.15f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
            player.GetCritChance<MeleeDamageClass>() += 5;
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
