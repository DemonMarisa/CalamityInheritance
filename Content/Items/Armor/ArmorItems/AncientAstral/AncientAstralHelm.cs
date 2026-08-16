using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientAstral
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAstralHelm : CIArmor
    {
        private const float DR = 0.12f;
        private const int LifeMaxSetBonus = 40;
        public const int CritsRegen = 25;
        public const int RogueCritsTimes = 25;
        public const float DefenseAndDR = 0.3f;
        public const float DefenseDamageReduction = 0.5f;
        public const int MaxStealthLifeRegenSpeed = 6;
        public const int LifeRegenSpeedResetCD = 15;
        public const float StealthRegen = 0.25f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.defense = 22;
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.lifeRegen += 2;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientAstralBreastplate>() && legs.type == ItemType<AncientAstralLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            player.CI().AncientAstralSet = true;
            player.SetRogueArmor(1.15f, true);
            player.lifeRegen += 4;
            player.LAP().MaxLifeAdditive += 40;
            player.AddDR(0.12f);
            player.pStone = true;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.MeteoriteBar, 10).
                    AddIngredient(CalamityMaterials.LifeAlloy, 5).
                    AddIngredient(CalamityMaterials.StarblightSoot, 10).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }

    }
}