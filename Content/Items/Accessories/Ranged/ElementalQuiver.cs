using System.Collections.Generic;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class ElementalQuiver : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Ranged";
        public static string TextPath => $"Mods.CalamityInheritance.Content.Items.Accessories.Ranged.ElementalQuiver";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<CalamityMod.Items.Accessories.ElementalQuiver>(false);
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.CIMod().IsWearingElemQuiverCal;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.defense = 30;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool allowSplit = CIConfig.Instance.ElementalQuiversplit;
            string t = Language.GetTextValue($"{TextPath}.DefaultText");
            if (!allowSplit)
            {
                tooltips.FindAndReplace("[TEXT]", t);
                return;
            }
            switch (CIConfig.Instance.ElementalQuiverSplitstyle)
            {
                case 1:
                    t = Language.GetTextValue($"{TextPath}.StyleOne");
                    break;
                case 2:
                    t = Language.GetTextValue($"{TextPath}.StyleTwo");
                    break;
                case 3:
                    t = Language.GetTextValue($"{TextPath}.StyleThree");
                    break;
                case 4:
                    t = Language.GetTextValue($"{TextPath}.StyleFour");
                    break;
            }
            tooltips.FindAndReplace("[TEXT]", t);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer1 = player.Calamity();
            CalamityInheritancePlayer modplayer = player.GetModPlayer<CalamityInheritancePlayer>();
            if (CIConfig.Instance.ElementalQuiversplit == true)
            {
                modplayer.ElemQuiver= true;
            }
            else
            {
                modplayer.ElemQuiver= false;
            }
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetCritChance(DamageClass.Ranged) += 20;
            player.ammoCost80 = true;
            player.lifeRegen += 4;
            player.pickSpeed -= 0.100f;
            player.magicQuiver = true;
            if (!modPlayer1.deadshotBrooch)
                modplayer.DeadshotBroochCI = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MagicQuiver).
                AddRecipeGroup(CIRecipeGroup.DaedalusEmblem).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
