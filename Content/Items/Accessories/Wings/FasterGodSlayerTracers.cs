﻿using CalamityMod.Items;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class FasterGodSlayerTracers: CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Wings";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 10.5f, 2.75f);
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<TracersElysian>(false);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if ((player.armor[0].type == ModContent.ItemType<AncientGodSlayerHelm>() ||
                 player.armor[0].type == ModContent.ItemType<GodSlayerHeadMeleeold>() ||
                 player.armor[0].type == ModContent.ItemType<GodSlayerHeadRangedold>() ||
                 player.armor[0].type == ModContent.ItemType<GodSlayerHeadMagicold>() ||
                 player.armor[0].type == ModContent.ItemType<GodSlayerHeadSummonold>() ||
                 player.armor[0].type == ModContent.ItemType<GodSlayerHeadRogueold>()) &&
                (player.armor[1].type == ModContent.ItemType<AncientGodSlayerChestplate>() ||
                 player.armor[1].type == ModContent.ItemType<GodSlayerChestplateold>()) &&
                (player.armor[2].type == ModContent.ItemType<AncientGodSlayerLeggings>() ||
                 player.armor[2].type == ModContent.ItemType<GodSlayerLeggingsold>()))
            {
                player.wingTime += 2.0f;
            }

            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                int dustXOffset = 4;
                if (player.direction == 1)
                {
                    dustXOffset = -40;
                }
                int flightDust = Dust.NewDust(new Vector2(player.position.X + player.width / 2 + dustXOffset, player.position.Y + player.height / 2 - 15f), 30, 30, Main.rand.NextBool() ? 206 : 173, 0f, 0f, 100, default, 2.4f);
                Main.dust[flightDust].noGravity = true;
                Main.dust[flightDust].velocity *= 0.3f;
                if (Main.rand.NextBool(10))
                {
                    Main.dust[flightDust].fadeIn = 2f;
                }
                Main.dust[flightDust].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
            }
            CalamityPlayer modPlayer = player.Calamity();
            player.accRunSpeed = 10.5f;
            player.rocketBoots = 3;
            player.moveSpeed += 0.2f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 240;
            player.buffImmune[BuffID.OnFire] = true;
            player.noFallDmg = true;
            modPlayer.tracersDust = !hideVisual;
            modPlayer.elysianWingsDust = !hideVisual;
            modPlayer.tracersElysian = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.95f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1.1f;
            maxAscentMultiplier = 3.15f;
            constantAscend = 0.135f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.TracersCelestial).
                AddIngredient<ElysianWings>().
                AddIngredient<CosmiliteBar>(5).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();


        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryCustomScale(
                spriteBatch,
                texture: TextureAssets.Item[Type].Value,
                position,
                frame,
                drawColor,
                itemColor,
                origin,
                scale,
                wantedScale: 0.9f,
                drawOffset: new(1f, 0f)
            );
            return false;
        }
    }
}
