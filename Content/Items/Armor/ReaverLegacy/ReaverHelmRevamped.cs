using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelmRevamped : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 12; //60 + (10) → 42 + (10)
            //Scarlet:保持伤害上限的基础上削弱其防御力，使其失去与日耀盔甲竞争的优势
            //不过要是有人拿着这套脆皮打亵渎的话我也没办法拦着他，他比较牛逼吧（
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ReaverScaleMailRevamped>() && legs.type == ModContent.ItemType<ReaverCuissesRevamped>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CIMod();
            modPlayer1.ReaverMeleeBlast = true;
            player.thorns += 0.33f;
            player.GetAttackSpeed<MeleeDamageClass>() +=0.30f;
            player.GetDamage<MeleeDamageClass>() += 0.05f;
            player.GetCritChance<MeleeDamageClass>() += 5;
            player.moveSpeed += 0.20f;
            //DemonMarisa:改了
            //Scarlet: 修复了永恒套提供的数值出错的问题
            //现在战士永恒套总加成：近战伤害25%+10%,，40+10攻速，15暴击
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.moveSpeed += 0.1f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
            player.GetDamage<MeleeDamageClass>() += 0.05f;
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
