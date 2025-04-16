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
    public class ReaverMaskRevamped : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceLime; 
            Item.rare = ItemRarityID.Lime;
            Item.defense = 5; //40 → 38 让他变得相对更脆
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
            CalamityPlayer modPlayer = player.Calamity();
            var modPlayer1 = player.CIMod();
            modPlayer1.ReaverMageBurst = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.GetDamage<MagicDamageClass>() += 0.10f;
            player.GetCritChance<MagicDamageClass>() += 10;
            //Scarlet:修复法师永恒套错误地提供了更高的魔力上线的数值(不小心给到80了)
            //永恒法师套现在提供60魔力上限，合计30+10%的伤害与25%的暴击概率
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage<MagicDamageClass>() += 0.10f;
            player.GetCritChance<MagicDamageClass>() += 5;
            player.manaCost *= 0.88f;
            player.moveSpeed += 0.1f;
            player.statManaMax2 += 60;
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
