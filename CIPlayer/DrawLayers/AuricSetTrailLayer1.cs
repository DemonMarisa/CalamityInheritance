using CalamityMod.Items.VanillaArmorChanges;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.CIPlayer.DrawLayers
{
    public class AuricSetTrailLayer1 : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.LastVanillaLayer);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || drawPlayer.dead || drawPlayer.Calamity().AdrenalineTrail || drawPlayer.Calamity().ascendantTrail)
                return false;

            return drawPlayer.Calamity().auricSet;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            List<DrawData> existingDrawData = drawInfo.DrawDataCache;
            float movementSpeedInterpolant = CobaltArmorSetChange.CalculateMovementSpeedInterpolant(drawPlayer);
            for (float i = 0f; i < drawPlayer.Calamity().OldPositions.Length; i += 1.7f)
            {
                float completionRatio = i / (float)drawPlayer.Calamity().OldPositions.Length;
                float scale = MathHelper.Lerp(1f, 0.6f, completionRatio);
                float opacity = MathHelper.Lerp(0.12f, 0.03f, completionRatio) * movementSpeedInterpolant;
                List<DrawData> afterimages = new List<DrawData>();
                for (int j = 0; j < existingDrawData.Count; j++)
                {
                    var drawData = existingDrawData[j];
                    drawData.position = existingDrawData[j].position - drawPlayer.position + drawPlayer.oldPosition;
                    drawData.color = new Color(105, 209, 248) * opacity;
                    drawData.scale = new Vector2(scale);
                    afterimages.Add(drawData);
                }
                drawInfo.DrawDataCache.InsertRange(0, afterimages);
            }
        }
    }
}
