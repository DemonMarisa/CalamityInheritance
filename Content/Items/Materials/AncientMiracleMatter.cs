using System;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    //旧奇迹物质造价更为昂贵，但是在作为合成材料时将不会被消耗。由于mod与灾厄的星流武器算起来数量已经较多
    //加入这一物品也算是一种qol
    public class AncientMiracleMatter : CIMaterials, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";
        public static float Convert01To010(float value) => (float)Math.Sin(MathHelper.Pi * MathHelper.Clamp(value, 0f, 1f));
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.SortingPriorityMaterials[Type] = 122;
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 80;
            Item.maxStack = 1;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<PureRed>();
        }

        public void DrawBackAfterimage(SpriteBatch spriteBatch, Vector2 baseDrawPosition, Rectangle frame, float baseScale)
        {
            if (Item.velocity.X != 0f)
                return;

            float pulse = (float)Math.Cos(1.61803398875f * Main.GlobalTimeWrappedHourly * 2f) + (float)Math.Cos(Math.E * Main.GlobalTimeWrappedHourly * 1.7f);
            pulse = pulse * 0.25f + 0.5f;

            // Sharpen the pulse with a power to give erratic fire bursts.
            pulse = (float)Math.Pow(pulse, 3D);

            float outwardnessFactor = MathHelper.Lerp(-0.3f, 1.2f, pulse);
            Color drawColor = Color.Lerp(new Color(255, 218, 99), new Color(249, 134, 44), pulse);
            drawColor *= MathHelper.Lerp(0.35f, 0.67f, Convert01To010(pulse));
            drawColor.A = 25;
            float drawPositionOffset = outwardnessFactor * baseScale * 8f;
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPosition = baseDrawPosition + (MathHelper.TwoPi * i / 8f).ToRotationVector2() * drawPositionOffset;
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, drawPosition, frame, drawColor, 0f, Vector2.Zero, baseScale, SpriteEffects.None, 0f);
            }
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();
            DrawBackAfterimage(spriteBatch, Item.position - Main.screenPosition, frame, scale);
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.velocity.X = 0f;
            DrawBackAfterimage(spriteBatch, position - frame.Size() * 0.25f, frame, scale);
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.94f * brightness, 0.95f * brightness, 0.56f * brightness);

            if (Main.rand.NextBool(3))
            {
                Dust exoFlame = Dust.NewDustDirect(Item.position, (int)(Item.width * Item.scale), (int)(Item.height * Item.scale * 0.6f), DustID.Torch);
                exoFlame.velocity = Vector2.Lerp(Main.rand.NextVector2Unit(), -Vector2.UnitY, 0.5f) * Main.rand.NextFloat(1.8f, 2.6f);
                exoFlame.scale *= Main.rand.NextFloat(0.85f, 1.15f);
                exoFlame.fadeIn = 0.9f;
                exoFlame.noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExoPrism>(10).
                AddIngredient<AuricBarold>(4).
                AddIngredient<CosmiliteBar>(10).
                AddIngredient<LifeAlloy>(10).
                AddIngredient<CoreofCalamity>(10).
                AddIngredient<AscendantSpiritEssence>(15).
                AddIngredient<GalacticaSingularity>(10).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<ExoPrism>(10).
                AddIngredient<AuricBar>(24).
                AddIngredient<CosmiliteBar>(10).
                AddIngredient<LifeAlloy>(10).
                AddIngredient<CoreofCalamity>(10).
                AddIngredient<AscendantSpiritEssence>(30).
                AddIngredient<GalacticaSingularity>(10).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
