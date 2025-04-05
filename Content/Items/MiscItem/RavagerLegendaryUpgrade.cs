using System.Drawing;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class RavagerLegendaryUpgrade: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Item.staff[Type]= true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 62;
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.maxStack = 1;
        }
        public override bool CanUseItem(Player player)
        {
            var p = player.CIMod();
            if (p.BetsyTier1)
            {
                string key = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.MiscItem.RavagerLegendaryUpgrade.Tint");
                Main.NewText(key, 155, 102, 4);
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
                return false;
            }
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.itemAnimation > 0 && player.itemTime ==0)
            {
                player.itemTime = Item.useTime;
                player.CIMod().BetsyTier1 = true;
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
            }
            return true;
        }
        public override void OnConsumeItem(Player player)
        {
            player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<RavagerLegendary>(), 1);
        }
    }
}