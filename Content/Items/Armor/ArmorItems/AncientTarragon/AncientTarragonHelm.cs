using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientTarragon
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientTarragonHelm : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 20; //90
        }


        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientTarragonBreastplate>() && legs.type == ItemType<AncientTarragonLeggings>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.SetRogueArmor(1.15f);
            player.CI().AncientTarragonSet = true;
            player.CI().BlockDefenseDamage = true;
            player.CI().LifeRegen += 8;
            player.LAP().MaxLifeMultiplier += 0.35f;
            player.LAP().healingPotionMult += 0.35f;
            player.noKnockback = true;
            player.lifeMagnet = true;
            if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                int getDef = player.statDefense;
                int buffDef = (int)(getDef * 0.2f);
                player.statDefense += buffDef;
                player.AddDR(0.2f);
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetCritChance<GenericDamageClass>() += 15;
            player.maxMinions += 3;
            player.maxTurrets += 2;
            player.LAP().MaxLifeAdditive += 150;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.UelibloomBar, 15).
                    AddIngredient(CalamityMaterials.DivineGeode, 15).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}