using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class PlanteraLegendaryUpgrade: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 62;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.maxStack = 9999;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.CIMod().PlanteraTier1)
            {
                string key = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.MiscItem.PlanteraLegendaryUpgrade.Tint");
                Main.NewText(key, 155, 102, 4);
                SoundEngine.PlaySound(CISoundID.SoundFallenStar, player.Center);
                return false;   
            }
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.itemAnimation > 0 && player.itemTime ==0)
            {
                player.itemTime = Item.useTime;
                player.CIMod().PlanteraTier1 = true;
                SoundEngine.PlaySound(CISoundID.SoundFallenStar, player.Center);
            }
            return true;
        }
        public override void OnConsumeItem(Player player)
        {
            player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<PlanteraLegendary>(), 1);
        }
    }
}