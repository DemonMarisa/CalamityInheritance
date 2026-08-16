using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientBloodflare
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientBloodflareMask : CIArmor, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.defense = 20; //80
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientBloodflareBodyArmor>() && legs.type == ItemType<AncientBloodflareCuisses>();
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.CI().AncientBloodflareSet = true;
            player.SetRogueArmor(1.2f);
            player.crimsonRegen = true;
            player.aggro += 900;
            //血炎数值
            player.LAP().healingPotionMult += 0.4f;
            player.lifeRegen += 10;
            if (player.statLife <= player.statLifeMax2 / 2)
                player.lifeRegen += 16;

        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 4;
            player.maxTurrets += 3;
            player.statLifeMax2 += 100;
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetCritChance<GenericDamageClass>() += 15f;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<BloodstoneCore>(15).
                    AddIngredient(CalamityMaterials.RuinousSoul, 15).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}