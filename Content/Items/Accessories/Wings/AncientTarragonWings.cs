using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories.Wings;
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
    public class AncientTarragonWings: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CalamityInheritance();
            if (player.armor[0].type == ModContent.ItemType<AncientTarragonHelm>() &&
                player.armor[1].type == ModContent.ItemType<AncientTarragonBreastplate>() &&
                player.armor[2].type == ModContent.ItemType<AncientTarragonLeggings>())
            {
                player.lifeRegen += 2; //1HP/s
                player.statDefense += 10; //10
            }
            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                float xOffset = 4f;
                if (player.direction == 1)
                {
                    xOffset = -40f;
                }
                int index = Dust.NewDust(new Vector2(player.Center.X + xOffset, player.Center.Y - 15f), 30, 30, DustID.Plantera_Green, 0f, 0f, 100, default, 2.4f);
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
                AddIngredient<UelibloomBar>(12).
                AddIngredient<DivineGeode>(12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}