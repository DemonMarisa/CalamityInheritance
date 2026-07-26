using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Graphics.DeepGlow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class CalamitousEssence : CIMaterials
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = RarityType<PureRed>();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Request<Texture2D>($"{Item.ModItem.Texture}_Glow").Value;
            Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.White, rotation, texture.Size() / 2, scale, 0, 0);
            DeepGlow.SubmitCustomGlow(() =>
            {
                Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.White, rotation, texture.Size() / 2, scale, 0, 0);
            });
        }
    }
}
