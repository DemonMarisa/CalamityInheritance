using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelmRevamped : CIArmor
    {

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
            return body.type == ItemType<ReaverScaleMailRevamped>() && legs.type == ItemType<ReaverCuissesRevamped>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.CI().ReaverMelee = true;
            player.thorns += 0.33f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.30f;
            player.GetDamage<MeleeDamageClass>() += 0.05f;
            player.GetCritChance<MeleeDamageClass>() += 5;
            player.moveSpeed += 0.20f;
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
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PerennialBar, 8).
                    AddIngredient(ItemID.JungleSpores, 8).
                    AddIngredient<CoreofEleum>(2).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.ChlorophyteBar, 8).
                    AddIngredient(ItemID.JungleSpores, 8).
                    AddIngredient<CoreofEleum>(2).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
