using CalamityInheritance.Content.BaseClass.Items;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.MiscDate;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Tools
{
    public class Moonlight : CITools
    {
        // Hardcoded times set by the vanilla Journey Mode buttons.
        // These are "halfway through day" and "halfway through night" respectively.
        private const int NoonCutoff = 43200;
        private const int MidnightCutoff = 16200;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 14; //加了一点使用时间来避免这东西被(我)按114514次
            Item.useTime = 14;
            Item.autoReuse = true;
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
                AddRecipeGroup(LAPRecipeGroup.AnyEvilBar, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
