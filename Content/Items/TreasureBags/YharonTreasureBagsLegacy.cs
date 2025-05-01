using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using MonoMod.ModInterop;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.TreasureBags
{
    public class YharonTreasureBagsLegacy : CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<YharonFire>() : ModContent.RarityType<DeepBlue>();
            //有意为之
            Item.value = CIShopValue.RarityPricePureRed;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.expert = true;
        }
        public override bool CanRightClick() => true;
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(new Fraction(16, 18), new int[]
            {
                ModContent.ItemType<DragonSword>(),
                ModContent.ItemType<BurningSkyLegacy>(),
                ModContent.ItemType<AncientDragonsBreath>(),
                ModContent.ItemType<ChickenCannonLegacy>(),
                ModContent.ItemType<DragonStaff>(),
                ModContent.ItemType<YharonSonStaff>(),
                ModContent.ItemType<DragonSpear>(),
                // 原灾武器
                ModContent.ItemType<DragonRage>(),
                ModContent.ItemType<TheBurningSky>(),
                ModContent.ItemType<ChickenCannon>(),
                ModContent.ItemType<DragonsBreath>(),
                ModContent.ItemType<PhoenixFlameBarrage>(),
                ModContent.ItemType<YharonsKindleStaff>(),
                ModContent.ItemType<TheFinalDawn>(),
                ModContent.ItemType<Wrathwing>(),
            }));

            //给点钱
            itemLoot.Add(ItemDropRule.Common(ItemID.PlatinumCoin, 1, 1, 3));
        }
    }
}