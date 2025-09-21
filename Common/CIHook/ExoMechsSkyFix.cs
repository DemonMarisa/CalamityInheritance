using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Skies;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CIHook
{
    public class ExoMechsSkyFix : ModSystem
    {
        public override void OnModLoad()
        {
            MethodInfo originalMethod = typeof(ExoMechsSky).GetMethod(nameof(ExoMechsSky.Update));
            MonoModHooks.Add(originalMethod, Update_Hook);
        }

        public static void Update_Hook(ExoMechsSky self, GameTime gameTime)
        {
            if (!ExoMechsSky.CanSkyBeActive && Main.LocalPlayer?.Calamity()?.monolithExoShader <= 0)
            {
                self.LightningIntensity = 0f;
                self.BackgroundIntensity = MathHelper.Clamp(self.BackgroundIntensity - 0.08f, 0f, 1f);
                self.LightningBolts.Clear();
                self.Deactivate(Array.Empty<object>());
                return;
            }

            self.LightningIntensity = MathHelper.Clamp(self.LightningIntensity * 0.95f - 0.025f, 0f, 1f);
            self.BackgroundIntensity = MathHelper.Clamp(self.BackgroundIntensity + 0.01f, 0f, 1f);

            for (int i = 0; i < self.LightningBolts.Count; i++)
            {
                self.LightningBolts[i].Lifetime--;
            }

            if (Main.rand.NextBool(125))
                ExoMechsSky.CreateLightningBolt();

            // Occasionally make the whole screen flash with lightning and create 7 bolts.
            if (Main.rand.NextBool(500))
            {
                self.LightningIntensity = 1f;
                ExoMechsSky.CreateLightningBolt(4);

                if (!Main.gamePaused)
                {
                    var lightningSound = SoundEngine.PlaySound(SoundID.Thunder with { Volume = SoundID.Thunder.Volume * 0.5f }, Main.LocalPlayer.Center);
                }
            }

            self.Opacity = self.BackgroundIntensity;
        }
    }
}
