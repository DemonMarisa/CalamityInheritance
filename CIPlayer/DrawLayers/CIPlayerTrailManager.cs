using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Graphics.Renderers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.DrawLayers
{
    public class CIPlayerTrailManager : ModSystem
    {
        public static bool CanDrawAfterImage = false;
        public override void Load()
        {
            On_Main.DrawPlayers_AfterProjectiles += DrawPlayers_AfterProjectiles;
        }
        public override void Unload()
        {
            On_Main.DrawPlayers_AfterProjectiles -= DrawPlayers_AfterProjectiles;
        }

        public static void DrawPlayers_AfterProjectiles(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
        {
            Player player = Main.player[Main.myPlayer];

            if (player.dead || player.Calamity().AdrenalineTrail || player.Calamity().ascendantTrail || player.mount.Active || !player.CIMod().AuricSilvaFakeDeath || player.sleeping.isSleeping)
            {
                orig(self);
                return;
            }

            CanDrawAfterImage = true;

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0, player.Center);

            Main.spriteBatch.End();

            CanDrawAfterImage = false;

            orig(self);
        }
    }
}
