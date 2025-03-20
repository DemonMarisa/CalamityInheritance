using CalamityMod.Items.Materials;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Rarity;
using Terraria.Audio;
using CalamityInheritance.Content.Items.Potions.CIPotions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class DraconicElixir : CIPotion, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public int frameCounter = 0;
        public int frame = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 44;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<DraconicSurgeBuff>();
            Item.buffTime = CalamityUtils.SecondsToFrames(480f);
            Item.value = CIShopValue.RarityPriceCatalystViolet;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Potions/DraconicElixir_Animated").Value;
            spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, 8, 10), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Potions/DraconicElixir_Animated").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 8, 10), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<YharonSoulFragment>().
                AddIngredient(ItemID.Daybloom).
                AddIngredient(ItemID.Moonglow).
                AddIngredient(ItemID.Fireblossom).
                AddTile(TileID.AlchemyTable).
                AddConsumeItemCallback(Recipe.ConsumptionRules.Alchemy).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<BloodOrb>(50).
                AddIngredient<YharonSoulFragment>().
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
