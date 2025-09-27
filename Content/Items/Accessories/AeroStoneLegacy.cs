using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AeroStoneLegacy : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:48,
            itemHeight:50,
            itemRare:ItemRarityID.Green,
            itemValue:CIShopValue.RarityPriceGreen
        );
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CIMod().AeroStonePower = true;
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
                wantedScale: 0.85f,
                drawOffset: new(0f, 0f)
            );
            return false;
        }
    }
}
