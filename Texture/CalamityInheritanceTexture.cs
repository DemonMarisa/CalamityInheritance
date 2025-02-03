using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Armor.Wulfum;
using CalamityInheritance.Content.Items.Tools;
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
    public class CalamityInheritanceTexture : ModPlayer
    {
        public static Asset<Texture2D> WulfrumAxeNew;

        public static Asset<Texture2D> WulfrumHammerNew;

        public static Asset<Texture2D> WulfrumPickaxeNew;

        public static Asset<Texture2D> WulfrumAxeOld;

        public static Asset<Texture2D> WulfrumHammerOld;

        public static Asset<Texture2D> WulfrumPickaxeOld;

        public static Asset<Texture2D> ArkoftheCosmosNew;

        public static Asset<Texture2D> ArkoftheCosmosOld;

        public static Asset<Texture2D> RampartofDeitiesNew;

        public static Asset<Texture2D> RampartofDeitiesOld;

        public static Asset<Texture2D> EtherealTalismanNew;

        public static Asset<Texture2D> EtherealTalismanOld;
        public static Asset<Texture2D> Skullmasher1p5;
        public static Asset<Texture2D> Skullmasher;
        public static void LoadTexture()
        {
            WulfrumAxeNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumAxeNew");
            WulfrumHammerNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumHammerNew");
            WulfrumPickaxeNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumPickaxeNew");

            WulfrumAxeOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumAxe");
            WulfrumHammerOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumHammer");
            WulfrumPickaxeOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumPickaxe");

            ArkoftheCosmosNew = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ArkoftheCosmosNew");
            ArkoftheCosmosOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/ArkoftheCosmosold");

            RampartofDeitiesNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/CIRampartofDeities");
            RampartofDeitiesOld = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/RampartofDeitiesOld");

            EtherealTalismanNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/Magic/EtherealTalisman");
            EtherealTalismanOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/Magic/AncientEtherealTalisman");
            Skullmasher1p5= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/Skullmasher1p5");
            Skullmasher   = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/Skullmasher");
        }
        public static void UnloadTexture()
        {
            WulfrumAxeNew = null;
            WulfrumHammerNew = null;
            WulfrumPickaxeNew = null;

            WulfrumAxeNew = null;
            WulfrumHammerNew = null;
            WulfrumPickaxeNew = null;

            ArkoftheCosmosNew = null;
            ArkoftheCosmosOld = null;

            RampartofDeitiesNew = null;
            RampartofDeitiesOld = null;

            EtherealTalismanNew = null;
            EtherealTalismanOld = null;

            Skullmasher1p5 = null;
            Skullmasher    = null;
        }
    }
}
