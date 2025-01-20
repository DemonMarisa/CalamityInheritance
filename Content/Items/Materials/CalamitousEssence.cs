using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public class CalamitousEssence : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<PureRed>();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Materials/CalamitousEssence_Glow").Value);
        }
    }
}
