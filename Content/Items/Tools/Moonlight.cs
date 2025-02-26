using CalamityMod.CalPlayer;
using CalamityMod.Items;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Tools
{
    public class Moonlight : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        // Hardcoded times set by the vanilla Journey Mode buttons.
        // These are "halfway through day" and "halfway through night" respectively.
        private const int NoonCutoff = 43200;
        private const int MidnightCutoff = 16200;

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 14; //加了一点使用时间来避免这东西被(我)按114514次
            Item.useTime = 14;
            Item.autoReuse = false;
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

            if (Main.dayTime)
                Main.SkipToTime(0, false);

            // Early Night -> Midnight
            else if (!Main.dayTime && Main.time < MidnightCutoff)
                Main.SkipToTime(MidnightCutoff, false);

            else if (!Main.dayTime && Main.time > MidnightCutoff)
                Main.SkipToTime(0, false);
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
