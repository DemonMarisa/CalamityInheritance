using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.World;
using CalamityMod;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.UI.HealthManaTextures
{
    public class CIResourceOverlay : ModResourceOverlay
    {
        // Most of this is taken from ExampleMod. See that for additional explanations.
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();
        public string baseFolder = "CalamityInheritance/UI/HealthManaTextures/";

        // Determines which health UI to draw based on player upgrades.
        public string LifeTexturePath()
        {
            string folder = $"{baseFolder}HP";
            CalamityPlayer modPlayer = Main.LocalPlayer.Calamity();
            CIWorld world = GetInstance<CIWorld>();
            if (world.IronHeart) // dozezoze - Chalice gets it's own heart color to make bleed indicator contrast consistent, and also because it looks cool
                return folder + "IronHeart";
            return string.Empty;
        }

        public override void PostDrawResource(ResourceOverlayDrawContext context)
        {
            Asset<Texture2D> asset = context.texture;
            // Vanilla texture paths
            string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";
            string barsFolder = "Images/UI/PlayerResourceSets/HorizontalBars/";

            if (LifeTexturePath() == string.Empty)
                return;

            // Draw hearts for Classic and Fancy
            if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2 || CompareAssets(asset, fancyFolder + "Heart_Fill") || CompareAssets(asset, fancyFolder + "Heart_Fill_B"))
            {
                context.texture = Request<Texture2D>(LifeTexturePath() + "Heart");
                context.Draw();
            }
            // Draw health bars
            else if (CompareAssets(asset, barsFolder + "HP_Fill") || CompareAssets(asset, barsFolder + "HP_Fill_Honey"))
            {
                context.texture = Request<Texture2D>(LifeTexturePath() + "Bar");
                context.Draw();
            }
        }
        private bool CompareAssets(Asset<Texture2D> currentAsset, string compareAssetPath)
        {
            if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
                asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(compareAssetPath);

            return currentAsset == asset;
        }
    }
}
