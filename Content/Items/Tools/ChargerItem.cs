using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using CalamityMod.Items.Materials;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Placeables.DraedonStructures;

namespace CalamityInheritance.Content.Items.Tools
{
    public class ChargerItem : ModItem, ILocalizedModType
    {
        public static readonly SoundStyle InstallSound = new("CalamityMod/Sounds/Custom/Codebreaker/AuricQuantumCoolingCellInstallNew");
        public new string LocalizationCategory => "Content.Items.Tools";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.rare = ItemRarityID.Red;

            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(InstallSound, Main.player[Main.myPlayer].Center);
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type > ItemID.Count && player.inventory[i].Calamity().UsesCharge)
                    player.inventory[i].Calamity().Charge = player.inventory[i].Calamity().MaxCharge;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<ChargingStationItem>()
            .AddIngredient<PowerCellFactoryItem>(2)
            .AddIngredient<DraedonPowerCell>(999)
            .AddIngredient<DubiousPlating>(50)
            .AddIngredient<MysteriousCircuitry>(25)
            .AddIngredient(ItemID.Glass, 50)
            .AddRecipeGroup("AnyCopperBar", 10)
            .AddIngredient(ItemID.Wire, 100)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
