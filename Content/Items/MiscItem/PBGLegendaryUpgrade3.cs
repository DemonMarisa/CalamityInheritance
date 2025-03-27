using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class PBGLegendaryUpgrade3: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override string Texture => "CalamityInheritance/Content/Items/MiscItem/PBGLegendaryUpgrade1";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 58;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 20;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.maxStack = 9999;
        }
        public override bool CanUseItem(Player player)
        {
            var p = player.CIMod();
            if (p.PBGLegendaryTier3)
                return false;
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.itemAnimation > 0 && player.itemTime ==0)
            {
                player.itemTime = Item.useTime;
                player.CIMod().PBGLegendaryTier3 = true;
            }
            return true;
        }
    }
}