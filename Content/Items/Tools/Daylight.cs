using CalamityInheritance.Content.BaseClass.Items;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.MiscDate;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Tools
{
    public class Daylight : CITools
    {
        // Hardcoded times set by the vanilla Journey Mode buttons.
        // These are "halfway through day" and "halfway through night" respectively.
        private const int NoonCutoff = 27000;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 50;
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.autoReuse = true; // Explicitly not autofire, since it can be used quickly now
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item60;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player) => !LAPInfo.AnyBossHere;

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
                AddRecipeGroup(LAPRecipeGroup.AnyEvilBar, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
