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
    public class ReaverCapRevamped : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 10;
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.SetRogueArmor(1.15f);
            player.CI().ReaverRogueSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage<ThrowingDamageClass>() += 0.15f;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.moveSpeed += 0.2f;
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
