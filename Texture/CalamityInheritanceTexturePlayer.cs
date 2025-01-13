using CalamityInheritance.Content.Items.Armor.Wulfum;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CalamityInheritanceTexturePlayer : ModPlayer
    {
        public static Asset<Texture2D> WulfumNewBody;

        public static Asset<Texture2D> WulfumOldBody;

    	public static void LoadTexture()
        {
            WulfumNewBody = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Armor/Wulfum/NewTexture/ANewWulfrumArmor");
            WulfumOldBody = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Armor/Wulfum/WulfrumArmorLegacy");
        }
        public static void UnloadTexture()
        {
            TextureAssets.Item[ModContent.ItemType<WulfrumArmorLegacy>()] = null;
            WulfumNewBody = null;
            WulfumOldBody = null;
        }
    }
}
