using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using MonoMod.ModInterop;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class YharonEssence: ModItem, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = CIConfig.Instance.SpecialRarityColor? ModContent.RarityType<YharonFire>() : ModContent.RarityType<DeepBlue>();
            //有意为之
            Item.value = CIShopValue.RarityPricePureRed;
            Item.consumable = true;
            Item.stack = 9999;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int[] dragonWeapons = 
            [
                ModContent.ItemType<DragonSword>(),
                ModContent.ItemType<BurningSkyLegacy>(),
                ModContent.ItemType<AncientDragonsBreath>(),
                ModContent.ItemType<ChickenCannonLegacy>(),
                ModContent.ItemType<DragonStaff>(),
                ModContent.ItemType<YharonSonStaff>(),
                ModContent.ItemType<DragonSpear>(),
            ];
            //随机给予2个
            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, dragonWeapons));
            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, dragonWeapons));
            //给点钱
            itemLoot.Add(ItemDropRule.Common(ItemID.PlatinumCoin, 1, 1, 3));
            //给予几乎超量的日食碎片
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DarksunFragment>(), 1, 120, 300));
        }
    }
}