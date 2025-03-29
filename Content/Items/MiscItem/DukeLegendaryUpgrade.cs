using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class DukeLegendaryUpgrade: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Item.staff[Type]= true;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<RavagerLegendaryUpgrade>());
        }

        public override bool CanUseItem(Player player)
        {
            if (player.CIMod().DukeTier1)
            {
                Main.NewText("你已经使用过这个物品了!", 155, 102, 4);
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
                player.CIMod().DukeTier1= true;
                SoundEngine.PlaySound(CISoundID.SoundFallenStar, player.Center);
            }
            return true;
        }
        public override void OnConsumeItem(Player player)
        {
            player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<DukeLegendary>(), 1);
        }
    }
}