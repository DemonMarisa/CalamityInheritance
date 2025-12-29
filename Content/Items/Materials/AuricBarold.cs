using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Rarity;
using CalamityMod.Tiles;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using CalamityInheritance.Tiles.Bars;

namespace CalamityInheritance.Content.Items.Materials
{
    public class AuricBarold : CIMaterials, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";
        public static Asset<Texture2D> GlowTexture { get; private set; }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                GlowTexture = Request<Texture2D>($"{Texture}_Glow");
            }
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 120;
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 15));
        }

        public override void Unload()
        {
            GlowTexture = null;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType<AuricBarLegacyTile>());
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-20f * player.direction, 15f * player.gravDir).RotatedBy(player.itemRotation);
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
                wantedScale: 1f,
                drawOffset: new(-1f, 0f)
            );
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            var frame = Item.GetFrame(whoAmI);
            var position = Item.Center - Main.screenPosition;
            var origin = frame.Size() / 2f;
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(GlowTexture.Value, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.5f * brightness, 0.7f * brightness, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient<AuricOre>(50).
                AddIngredient<LifeAlloy>(10).
                AddIngredient<YharonSoulFragment>(5).
                AddIngredient<UnholyEssence>(10).
                AddIngredient<CoreofCalamity>(5).
                AddIngredient<AscendantSpiritEssence>(5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
