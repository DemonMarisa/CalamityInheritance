using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.NPCs.Boss.CalamitasClone;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.CalClone;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.SummonItems
{
    public class RebornEyeSummon: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 28;
            Item.noMelee = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 1;
            Item.rare = ModContent.RarityType<PureRed>();
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool CanUseItem(Player player)
        {
            return NPC.AnyNPCs(ModContent.NPCType<CalamitasCloneLegacy>()) && !Main.dayTime;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
            
                int getBoss = ModContent.NPCType<CalamitasCloneLegacy>();

                if (Main.netMode!=NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, getBoss);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent,  number: player.whoAmI, number2: getBoss);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EssenceofHavoc>(10).
                AddIngredient(ItemID.HellstoneBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}