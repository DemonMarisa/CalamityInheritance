using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientSilva
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientSilvaHelm : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 20; //100
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientSilvaArmor>() && legs.type == ItemType<AncientSilvaLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            player.SetRogueArmor(1.25f);
            player.LAP().healingPotionMult += 0.3f;
            player.CI().AncientSilvaSet = true;
            player.CI().SilvaReborn = true;
            player.lifeRegen += 24; //+12HP/s
            player.lifeRegenTime = 2000;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
            player.maxTurrets += 2;
            player.statLifeMax2 += 200;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PlantyMush, 32).
                    AddIngredient(CalamityMaterials.EffulgentFeather, 50).
                    AddIngredient(CalamityMaterials.DarksunFragment, 25).
                    AddIngredient<LeadCore>().
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}