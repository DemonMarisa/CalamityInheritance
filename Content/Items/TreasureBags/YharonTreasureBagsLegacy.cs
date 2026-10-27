using CalamityInheritance.Content.Items.Weapons.Magic.Staff;
using CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords;
using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityInheritance.Content.Items.Weapons.Ranged.Assault;
using CalamityInheritance.Content.Items.Weapons.Ranged.Launcher;
using CalamityInheritance.Content.Items.Weapons.Rogue.Spear;
using CalamityInheritance.Content.Items.Weapons.Summon.Normal.CloseRange;
using CalamityInheritance.Content.NPCs.CILoots;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using CalamityInheritance.Core.Path;
using CalamityMod;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.TreasureBags
{
    public class YharonTreasureBagsLegacy : ModItem
    {
        public override string LocalizationCategory => LocalizationPath.TreasureBag;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = RarityType<YharonFire>();
            Item.value = CIShopValue.RarityPricePureRed;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.expert = true;
        }
        public override bool CanRightClick() => true;
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int [] items = [ItemType<DragonSword>(), ItemType<BurningSkyLegacy>(), ItemType<AncientDragonsBreath>(), ItemType<ChickenCannonLegacy>(),
                ItemType<DragonStaff>(), ItemType<YharonSonStaff>(), ItemType<DragonSpear>()];
            itemLoot.Add(ItemDropRule.FewFromOptions(3, 1, items));
            itemLoot.Add(ItemDropRule.Common(ItemID.PlatinumCoin, 1, 1, 3));
        }
    }
}