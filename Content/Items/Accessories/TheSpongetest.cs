﻿using CalamityMod.CalPlayer;
using CalamityMod.DataStructures;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.World;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Graphics.Effects;
using CalamityInheritance;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Media;

namespace CalamityInheritance.Content.Items.Accessories
{

    public class TheSpongetest : ModItem, ILocalizedModType, IDyeableShaderRenderer
    {
        public new string LocalizationCategory => "Items.Accessories";

        public override string Texture => (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) ? "CalamityMod/Items/Accessories/TheSpongeReal" : "CalamityMod/Items/Accessories/TheSponge";

        public static Asset<Texture2D> NoiseTex;
        // TODO -- Unique sounds for The Sponge
        public static readonly SoundStyle ShieldHurtSound = new("CalamityMod/Sounds/Custom/RoverDriveHit") { PitchVariance = 0.6f, Volume = 0.6f, MaxInstances = 0 };
        public static readonly SoundStyle ActivationSound = new("CalamityMod/Sounds/Custom/RoverDriveActivate") { Volume = 0.85f };
        public static readonly SoundStyle BreakSound = new("CalamityMod/Sounds/Custom/RoverDriveBreak") { Volume = 0.75f };

        public static int CIShieldDurabilityMax
        {
            get
            {
                if (Main.LocalPlayer != null)
                {
                    return Main.LocalPlayer.statLifeMax2;
                }
                return 800;
            }
        }
        public static int CIShieldRechargeDelay = CalamityUtils.SecondsToFrames(10); // was 6
        public static int CIShieldRechargeRelay = CalamityUtils.SecondsToFrames(5);
        public static int CITotalShieldRechargeTime = CalamityUtils.SecondsToFrames(6);

        // While active, The Sponge gives 30 defense and 10% DR
        public static int ShieldActiveDefense = 30;
        public static float ShieldActiveDamageReduction = 0.3f;

        public float RenderDepth => IDyeableShaderRenderer.SpongeShieldDepth;

        public bool ShouldDrawDyeableShader
        {
            get
            {
                bool result = false;
                foreach (Player player in Main.ActivePlayers)
                {
                    if (player.outOfRange || player.dead)
                        continue;

                    CalamityPlayer modPlayer = player.Calamity();
                    CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();

                    // Do not render the shield if its visibility is off (or it does not exist)
                    bool isVanityOnly = modPlayer1.CIspongeShieldVisible && !modPlayer1.CIsponge;
                    bool shieldExists = isVanityOnly || modPlayer1.CISpongeShieldDurability > 0;
                    bool shouldntDraw = !modPlayer.spongeShieldVisible || modPlayer.drawnAnyShieldThisFrame || !shieldExists;
                    result |= !shouldntDraw;
                }
                return result;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 30));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            if (CalamityInheritanceConfig.Instance.TheSpongeBarrier == true)
            {
                modPlayer1.CIsponge = true;
            }
            else
            {
                modPlayer1.CIsponge = false;
            }
            if (modPlayer1.CISpongeShieldDurability > 0)
            {
                player.statDefense += ShieldActiveDefense;
                player.endurance += ShieldActiveDamageReduction;
            }
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.spongeShieldVisible = !hideVisual;
            modPlayer.aAmpoule = true;
            modPlayer.alwaysHoneyRegen = true;
            modPlayer.honeyDewHalveDebuffs = true;
            modPlayer.livingDewHalveDebuffs = true;
            modPlayer.aSpark = true;
            modPlayer.gShell = true;
            modPlayer1.FungalCarapace = true;
            player.endurance += 0.15f;
            player.statDefense += 30;
            player.statManaMax2 += 30;
            player.buffImmune[ModContent.BuffType<ArmorCrunch>()] = true;
            player.statLifeMax += 30;
            if (CalamityInheritanceConfig.Instance.TheSpongeBarrier == true)
            {
                if (modPlayer1.CISpongeShieldDurability == 0)
                {
                    player.endurance -= 0.25f;
                    player.statDefense -= 50;
                }
            }
            else
            {
                player.endurance -= 0f;
                player.statDefense -= 0;
            }
        }

        // In vanity, provides a visual shield but no actual functionality
        public override void UpdateVanity(Player player) => player.Calamity().spongeShieldVisible = true;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string adrenTooltip = CalamityWorld.revenge ? this.GetLocalizedValue("ShieldAdren") : "";
            tooltips.FindAndReplace("[ADREN]", adrenTooltip);
        }

        // Renders the bubble shield over the item in the world.
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            // Will not render a shield if the April Fool's sprite is active.
            if (Texture == "CalamityMod/Items/Accessories/TheSponge")
            {
                Texture2D tex = ModContent.Request<Texture2D>("CalamityMod/Items/Accessories/TheSpongeShield").Value;
                spriteBatch.Draw(tex, Item.Center - Main.screenPosition + new Vector2(0f, 0f), Main.itemAnimations[Item.type].GetFrame(tex), Color.Cyan * 0.5f, 0f, new Vector2(tex.Width / 2f, (tex.Height / 30f) * 0.8f), 1f, SpriteEffects.None, 0);
            }
        }

        // TODO -- Is this necessary to block the shield in-inventory on April first?
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return Texture != "CalamityMod/Items/Accessories/TheSponge";
        }

        // Renders the bubble shield over the item in a player's inventory.
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // Will not render a shield if the April Fool's sprite is active.
            if (Texture == "CalamityMod/Items/Accessories/TheSponge")
            {
                float wantedScale = 0.85f;
                Vector2 drawOffset = new(-2f, -1f);

                CalamityUtils.DrawInventoryCustomScale(
                    spriteBatch,
                    texture: TextureAssets.Item[Type].Value,
                    position,
                    frame,
                    drawColor,
                    itemColor,
                    origin,
                    scale,
                    wantedScale,
                    drawOffset
                );
                Texture2D tex = ModContent.Request<Texture2D>("CalamityMod/Items/Accessories/TheSpongeShield").Value;
                CalamityUtils.DrawInventoryCustomScale(
                    spriteBatch,
                    texture: tex,
                    position,
                    Main.itemAnimations[Item.type].GetFrame(tex),
                    Color.Cyan * 0.4f,
                    itemColor,
                    origin,
                    scale,
                    wantedScale,
                    drawOffset
                );
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RoverDrive>().
                AddIngredient<MysteriousCircuitry>(10).
                AddIngredient<DubiousPlating>(20).
                AddIngredient<CosmiliteBar>(5).
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient<FungalCarapace>().
                AddIngredient<AmbrosialAmpoule>().
                AddTile<CosmicAnvil>().
                Register();
        }

        // Complex drawcode which draws Sponge shields on ALL players who have it available. Supposedly.
        // This is applied as IL (On hook) which draws right before Inferno Ring.
        public void DrawDyeableShader(SpriteBatch spriteBatch)
        {
            // TODO -- 控制流分析表明这个钩子不稳定（因为它是从 Rover Drive 复制过来的）。
            // 对于每个装备了海绵的玩家，都会绘制海绵护盾。
            // 但是，无法保证每个玩家的护盾都处于正确的状态。
            // 例如，护盾的可见性不是网络同步的。
            bool alreadyDrawnShieldForPlayer = false;

            foreach (Player player in Main.ActivePlayers)
            {
                if (player.outOfRange || player.dead)
                    continue;

                CalamityPlayer modPlayer = player.Calamity();
                CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();

                // 如果护盾的可见性关闭（或不存在），则不渲染护盾
                bool isVanityOnly = modPlayer.spongeShieldVisible && !modPlayer1.CIsponge;
                bool shieldExists = isVanityOnly || modPlayer1.CISpongeShieldDurability > 0;
                if (!modPlayer.spongeShieldVisible || modPlayer.drawnAnyShieldThisFrame || !shieldExists)
                    continue;

                // 缩放绘制护盾的比例。海绵护盾会轻微地变大和缩小，这种变化应该几乎无法察觉。
                // "i" 参数是为了让不同玩家的护盾不会完全同步。
                int i = player.whoAmI;
                float baseScale = 0.155f;
                float maxExtraScale = 0.025f;
                float extraScalePulseInterpolant = MathF.Pow(4f, MathF.Sin(Main.GlobalTimeWrappedHourly * 0.791f + i) - 1);
                float scale = baseScale + maxExtraScale * extraScalePulseInterpolant;

                if (!alreadyDrawnShieldForPlayer)
                {
                    // If in vanity, the shield is always projected as if it's at full strength.
                    float visualShieldStrength = 1f;
                    if (!isVanityOnly)
                    {
                        // Again, I believe there is no way this looks correct when two players have The Sponge equipped.
                        CalamityInheritancePlayer localModPlayer = Main.LocalPlayer.CalamityInheritance();
                        float shieldDurabilityRatio = localModPlayer.CISpongeShieldDurability / (float)CIShieldDurabilityMax;
                        visualShieldStrength = MathF.Pow(shieldDurabilityRatio, 0.5f);
                    }

                    // The scale used for the noise overlay also grows and shrinks
                    // This is intentionally out of sync with the shield, and intentionally desynced per player
                    // Don't put this anywhere less than 0.15f or higher than 0.75f. The higher it is, the denser / more zoomed out the noise overlay is.
                    // Changing this too quickly and/or too much makes the noise grow and shrink visibly, so be careful with that.
                    float noiseScale = MathHelper.Lerp(0.28f, 0.38f, 0.5f + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 0.347f + i));

                    // Define shader parameters
                    Effect shieldEffect = Filters.Scene["CalamityMod:RoverDriveShield"].GetShader().Shader;
                    shieldEffect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * 0.0813f); // Scrolling speed of polygonal overlay
                    shieldEffect.Parameters["blowUpPower"].SetValue(3f);
                    shieldEffect.Parameters["blowUpSize"].SetValue(0.56f);
                    shieldEffect.Parameters["noiseScale"].SetValue(noiseScale);

                    // Shield opacity multiplier slightly changes, this is independent of current shield strength
                    float baseShieldOpacity = 0.9f + 0.1f * MathF.Sin(Main.GlobalTimeWrappedHourly * 1.95f);
                    float minShieldStrengthOpacityMultiplier = 0.25f;
                    float finalShieldOpacity = baseShieldOpacity * MathHelper.Lerp(minShieldStrengthOpacityMultiplier, 1f, visualShieldStrength);
                    shieldEffect.Parameters["shieldOpacity"].SetValue(finalShieldOpacity);
                    shieldEffect.Parameters["shieldEdgeBlendStrenght"].SetValue(4f);

                    Color shieldColor = new Color(24, 156, 204); // #189CCC
                    Color primaryEdgeColor = shieldColor;
                    Color secondaryEdgeColor = new Color(34, 224, 227); // #22E0E3                   

                    // Final shield edge color, which lerps about
                    Color edgeColor = CalamityUtils.MulticolorLerp(Main.GlobalTimeWrappedHourly * 0.2f, primaryEdgeColor, secondaryEdgeColor);

                    // Define shader parameters for shield color
                    shieldEffect.Parameters["shieldColor"].SetValue(shieldColor.ToVector3());
                    shieldEffect.Parameters["shieldEdgeColor"].SetValue(edgeColor.ToVector3());

                    // GOD I LOVE END BEGIN CAN THIS GAME PLEASE BE SWALLOWED BY THE FIRES OF HELL THANKS
                    // yes I copy pasted that comment, I hate end begin that much
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shieldEffect, Main.GameViewMatrix.TransformationMatrix);
                }

                alreadyDrawnShieldForPlayer = true;
                modPlayer.drawnAnyShieldThisFrame = true;

                // Fetch shield noise overlay texture (this is the polygons fed to the shader)
                NoiseTex ??= ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/GreyscaleGradients/TechyNoise");
                Vector2 pos = player.MountedCenter + player.gfxOffY * Vector2.UnitY - Main.screenPosition;
                Texture2D tex = NoiseTex.Value;
                Main.spriteBatch.Draw(tex, pos, null, Color.White, 0, tex.Size() / 2f, scale, 0, 0);
            }

            if (alreadyDrawnShieldForPlayer)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            }
        }
    }
}