using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class AncientXerocWings: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Wings";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modplayer1 = player.CalamityInheritance();

            if(modplayer1.ancientXerocSet)
            modplayer1.ancientXerocMadness = true;
            //与克希洛克盔甲搭配时提供克希洛克之怒效果

            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                float xOffset = 4f;
                if (player.direction == 1)
                {
                    xOffset = -40f;
                }
                int index = Dust.NewDust(new Vector2(player.Center.X + xOffset, player.Center.Y - 15f), 30, 30, DustID.PurpleTorch, 0f, 0f, 100, default, 2.4f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.3f;
                if (Main.rand.NextBool(10))
                {
                    Main.dust[index].fadeIn = 2f;
                }
                Main.dust[index].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
            }
            player.wingTimeMax = 180;
            player.noFallDmg = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GalacticaSingularity>(5).
                AddIngredient<NebulaBar>(9).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
