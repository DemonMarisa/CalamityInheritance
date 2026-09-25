using CalamityInheritance.Assets;
using CalamityInheritance.Assets.Effects;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Accessories.Restorative;
using CalamityInheritance.Content.Particles;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Graphics.Insertlayer;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class TheSpongeLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        // public static int CIShieldDurabilityMax => Main.LocalPlayer?.GetModPlayer<CalamityInheritancePlayer>()?.ShieldDurabilityMax ?? 0;
        public static int CIShieldRechargeDelay = SecondsToFrames(15); // was 6
        public static int CIShieldRechargeRelay = SecondsToFrames(9);
        public static int CITotalShieldRechargeTime = SecondsToFrames(15);
        // While active, The Sponge gives 30 defense and 10% DR
        public static int ShieldActiveDefense = 30;
        public static float ShieldActiveDamageReduction = 0.3f;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 30));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30;
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            player.AddCD(LAPContent.CDType<EnergyShieldCD>(), CIAccessoriesBlance.CIShieldRechargeDelay);
            return true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.CI().AnyShield = true;
            player.CI().CIsponge = true;
            player.pickSpeed *= 0.5f;
            player.LAP().MaxLifeAdditive += 30;
            player.LAP().MaxManaAdditive += 30;
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += 0.5f;
            player.AddDR(0.15f);
            player.buffImmune[BuffType<CIArmorCrunch>()] = true;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.ArmorCrunch] = true;
            // 海螺壳
            player.ignoreWater = true;
            if (player.IsUnderwater())
            {
                player.statDefense += 12;
                player.endurance += 0.10f;
                player.moveSpeed += 0.20f;
            }
            player.CI().FungalCarapaceHurt = true;
            // 炫彩凝胶
            if (MathF.Abs(player.velocity.X) < 0.05f && MathF.Abs(player.velocity.Y) < 0.05f)
            {
                player.lifeRegen += 2;
                player.manaRegenBonus += 2;
            }
            // 百草瓶
            player.honey = true;
            player.CI().BeeFriendly = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Frostburn2] = true;
            // 阴阳石
            player.CI().AmidiasSpark = true;
            player.CI().HurtHeal += 0.1f;
        }
        public override void UpdateVanity(Player player)
        {
            player.CI().CIspongeVanity = true;
        }
        public override void UpdateVisibleAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                if (player.CI().stateShield == 0 && !player.CI().CIspongeVanity)
                    return;
                if (player.outOfRange || player.dead)
                    return;
                float baseScale = 0.155f;
                float maxExtraScale = 0.025f;
                float extraScalePulseInterpolant = MathF.Pow(4f, MathF.Sin(Main.GlobalTimeWrappedHourly * 0.791f) - 1);
                float scale = baseScale + maxExtraScale * extraScalePulseInterpolant;
                float noiseScale = MathHelper.Lerp(0.28f, 0.38f, 0.5f + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 0.347f));
                InsertDraw.SubmitDrawRequest_APlayer("SpongeShieldDraw",() =>
                {
                    LAPUtilities.ReSetToBeginShader();
                    Effect shieldEffect = CIShaders.RoverDriveShield.Value;
                    shieldEffect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * 0.0813f); // Scrolling speed of polygonal overlay
                    shieldEffect.Parameters["blowUpPower"].SetValue(3f);
                    shieldEffect.Parameters["blowUpSize"].SetValue(0.56f);
                    shieldEffect.Parameters["noiseScale"].SetValue(noiseScale * 2f);
                    float baseShieldOpacity = 0.9f + 0.1f * MathF.Sin(Main.GlobalTimeWrappedHourly * 1.95f);
                    float minShieldStrengthOpacityMultiplier = 0.25f;
                    float finalShieldOpacity = baseShieldOpacity * MathHelper.Lerp(minShieldStrengthOpacityMultiplier, 1f, 1f);
                    shieldEffect.Parameters["shieldOpacity"].SetValue(finalShieldOpacity);
                    shieldEffect.Parameters["shieldEdgeBlendStrenght"].SetValue(4f);
                    Color shieldColor = new Color(24, 156, 204); // #189CCC
                    Color primaryEdgeColor = shieldColor;
                    Color secondaryEdgeColor = new Color(34, 224, 227); // #22E0E3                   
                    Color edgeColor = MulticolorLerp(Main.GlobalTimeWrappedHourly * 0.2f, primaryEdgeColor, secondaryEdgeColor);
                    shieldEffect.Parameters["shieldColor"].SetValue(shieldColor.ToVector3());
                    shieldEffect.Parameters["shieldEdgeColor"].SetValue(edgeColor.ToVector3());
                    shieldEffect.CurrentTechnique.Passes[0].Apply();
                    Vector2 pos = player.MountedCenter + player.gfxOffY * Vector2.UnitY - Main.screenPosition;
                    Texture2D tex = CIExtraTexture.TechyNoise.Value;
                    Main.spriteBatch.Draw(tex, pos, null, Color.White, 0, tex.Size() / 2f, scale, 0, 0);
                    LAPUtilities.ReSetToEndShader();
                });
            }
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
            {
                Texture2D texture = Request<Texture2D>(Texture + "Fools").Value;
                Rectangle Foolsframe = texture.Frame(1, 30, 0, 0);
                LAPUtilities.Draw(texture, Item.position + Vector2.UnitY * 2, Foolsframe, lightColor, 0, Foolsframe.Size() / 2, scale * 2, 0);
                return false;
            }
            else
                return true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
            {
                Texture2D texture = Request<Texture2D>(Texture + "Fools").Value;
                Rectangle Foolsframe = texture.Frame(1, 30, 0, 0);
                LAPUtilities.Draw(texture, position + Vector2.UnitY * 2, Foolsframe, drawColor, 0, Foolsframe.Size() / 2, scale * 2, 0);
                return false;
            }
            else
                return true;
        }
        public override void AddRecipes()
        {
            if (HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<TheAbsorberOld>().
                    AddIngredient<AmbrosialAmpouleOld>().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 15).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
        }
        public static void TheSponge_Hurt(Player player, bool broken, in Player.HurtInfo info)
        {
            int numParticles = Main.rand.Next(24, 28) + (broken ? 16 : 0);
            for (int i = 0; i < numParticles; i++)
            {
                float maxVelocity = 36f;
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(8f, maxVelocity);
                velocity.X += 5f * info.HitDirection;

                float scale = Main.rand.NextFloat(4f, 7f);
                Color particleColor = Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247);
                int lifetime = Main.rand.Next(35, 60);

                new TechyHolosquareParticle(player.Center, velocity, scale, particleColor, lifetime).Spawn();
            }
        }
    }
}
