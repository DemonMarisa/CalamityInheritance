using CalamityInheritance.CIPlayer;
using CalamityInheritance.System;
using CalamityMod.CalPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityModMusic.Items.Placeables;
using CalamityMod.Systems;

namespace CalamityInheritance.Content.Items.Tools
{
    public class MusicMarkChange : ModItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public new string LocalizationCategory => "Content.Items.Tools";

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 1; //55->1 这玩意前期能变真近战武器而非调试工具了
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.LightRed;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            if (player.altFunctionUse == 2)
            {
                CIMusicEventSystem.PlayedEvents.Add("YharonDefeated");
                CIMusicEventSystem.PlayedEvents.Add("ExoMechsDefeated");
                //灾厄的音乐事件
                MusicEventSystem.PlayedEvents.Add("CloneDefeated");
                MusicEventSystem.PlayedEvents.Add("MLDefeated");
                MusicEventSystem.PlayedEvents.Add("YharonDefeated");
                MusicEventSystem.PlayedEvents.Add("DoGDefeated");
                Main.NewText("添加所有音乐事件标记");
            }
            else
            {
                CIMusicEventSystem.PlayedEvents.Remove("YharonDefeated");
                CIMusicEventSystem.PlayedEvents.Remove("ExoMechsDefeated");
                CIMusicEventSystem.PlayedEvents.Remove("ScalDefeatedL");
                //灾厄的音乐事件
                MusicEventSystem.PlayedEvents.Remove("CloneDefeated");
                MusicEventSystem.PlayedEvents.Remove("MLDefeated");
                MusicEventSystem.PlayedEvents.Remove("YharonDefeated");
                MusicEventSystem.PlayedEvents.Remove("DoGDefeated");
                Main.NewText("清除所有音乐事件标记");
            }
            return base.CanUseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<CalamityTitleMusicBox>().
            AddTile(TileID.WorkBenches).
            Register();
        }
    }
}
