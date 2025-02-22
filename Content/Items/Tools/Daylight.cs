using CalamityMod.CalPlayer;
using CalamityMod.Items;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Tools
{
    public class Daylight : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        // Hardcoded times set by the vanilla Journey Mode buttons.
        // These are "halfway through day" and "halfway through night" respectively.
        private const int NoonCutoff = 27000;

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 50;
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 9;
            Item.useTime = 9;
            Item.autoReuse = false; // Explicitly not autofire, since it can be used quickly now
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item60;
            Item.consumable = false;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.ToolsOther;
        }

        public override bool CanUseItem(Player player) => !CalamityPlayer.areThereAnyDamnBosses;

        public override bool? UseItem(Player player)
        {
            //Only SinglePlayer or DedServ should change time to prevent unwanted race condition
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            // Early Morning -> Noon
            if (Main.dayTime && Main.time > NoonCutoff)
                Main.SkipToTime(0, true);

            // Afternoon -> Dusk
            else if (Main.dayTime)
                Main.SkipToTime(NoonCutoff, true);

            // Late Night -> Dawn
            else if (!Main.dayTime)
                Main.SkipToTime(0, true);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DemoniteBar, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
