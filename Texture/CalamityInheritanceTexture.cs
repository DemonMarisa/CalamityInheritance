using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Armor.Wulfum;
using CalamityInheritance.Content.Items.Tools;
using CalamityMod.CalPlayer.Dashes;
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
        #region 贴图火车
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

        public static Asset<Texture2D> P90;

        public static Asset<Texture2D> P90Legacy;

        #region 各种飞刀的贴图
        public static Asset<Texture2D> GodSlayerKnivesLegacyType; //苍穹飞刀的第二版本贴图(现在的版本)
        public static Asset<Texture2D> GodSlayerKnivesLegacyTypeProj; //苍穹飞刀的第二版射弹(现在的版本)
        //下面的编排与上方的一样。
        public static Asset<Texture2D> GodSlayerKnivesAlterType;  //苍穹飞刀的初版贴图
        public static Asset<Texture2D> GodSlayerKnivesAlterTypeProj;
        public static Asset<Texture2D> ShadowspecKnivesLegacyType; //圣光飞刀的第三版本贴图(灾厄现在的是第四版)
        public static Asset<Texture2D> ShadowspecKnivesLegacyTypeProj;
        public static Asset<Texture2D> ShadowspecKnivesAlterType;  //圣光飞刀的第二版本贴图
        public static Asset<Texture2D> ShadowspecKnivesAlterTypeProj;
        public static Asset<Texture2D> ShadowspecKnivesAlterTypeSecond; //圣光飞刀的初版贴图
        public static Asset<Texture2D> ShadowspecKnivesAlterTypeProjSecond;
        #endregion
        #endregion
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

            P90 = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/P90");
            P90Legacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/P90Legacy");

            /*下方为飞刀的各种贴图，排版与上述定义时相同*/
            //苍穹飞刀(现)
            GodSlayerKnivesLegacyType = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/EmpyreanKnivesLegacyRogue");
            GodSlayerKnivesLegacyTypeProj= ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Rogue/EmpyreanKnivesProjectileLegacyRogue");
            //苍穹飞刀(初)
            GodSlayerKnivesAlterType = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesLegacyRogueAlterStyle1");
            GodSlayerKnivesAlterTypeProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesProjectileLegacyRogueAlterStyle1");
            //圣光飞刀(三)
            ShadowspecKnivesLegacyType = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/ShadowspecKnivesLegacyRogue");
            ShadowspecKnivesLegacyTypeProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Rogue/ShadowspecKnivesProjectileLegacyRogue");
            //圣光飞刀(二)
            ShadowspecKnivesAlterType= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesLegacyRogueAlterStyle1");
            ShadowspecKnivesAlterTypeProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesProjectileLegacyRogueAlterStyle1");
            //圣光飞刀(初)
            ShadowspecKnivesAlterTypeSecond = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesLegacyRogueAlterStyle2");
            ShadowspecKnivesAlterTypeProjSecond = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesProjectileLegacyRogueAlterStyle2");

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

            P90 = null;
            P90Legacy = null;

            //飞刀，排版同上
            GodSlayerKnivesLegacyType = null;
            GodSlayerKnivesLegacyTypeProj = null;

            GodSlayerKnivesAlterType = null;
            GodSlayerKnivesAlterTypeProj = null;

            ShadowspecKnivesLegacyType = null;
            ShadowspecKnivesLegacyTypeProj = null;

            ShadowspecKnivesAlterType = null;
            ShadowspecKnivesAlterTypeProj = null;

            ShadowspecKnivesAlterTypeSecond = null;
            ShadowspecKnivesAlterTypeProjSecond = null;
        }
    }
}
