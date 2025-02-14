using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class HeavenlyGaleold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public const float NormalArrowDamageMult = 1.25f;
        private static int[] ExoArrows;
        private static int[] ExoArrowsExoLore;
        public override void SetStaticDefaults()
        {
            ExoArrows =
            [
            ModContent.ProjectileType<ExoArrowTeal>(),
            ModContent.ProjectileType<OrangeExoArrow>(),
            ModContent.ProjectileType<ExoArrowGreen>(),
            ModContent.ProjectileType<ExoArrowBlue>()
            ];
            ExoArrowsExoLore =
            [
            ModContent.ProjectileType<ExoArrowTealExoLore>(),
            ModContent.ProjectileType<ExoArrowOrangeExoLore>(),
            ModContent.ProjectileType<ExoArrowOrangeExoLore>(),
            ModContent.ProjectileType<ExoArrowGreenExoLore>(),
            ModContent.ProjectileType<ExoArrowBlueExoLore>()
            ];
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 198;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 98;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override bool CanUseItem(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            if (usPlayer.exoMechLore)
            {
                Item.damage = 298;
            }
            else
            {
                Item.damage = 198;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();

            Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 baseOffset = velocity;
            baseOffset.Normalize();
            baseOffset *= 40f;

            float piOver10 = MathHelper.Pi / 10f;
            bool againstWall = !Collision.CanHit(source, 0, 0, source + baseOffset, 0, 0);

            int numArrows = 5;
            float dmgMult = 1f;

            for (int i = 0; i < numArrows; i++)
            {
                float offsetAmt = i - (numArrows - 1f) / 2f;
                Vector2 offset = baseOffset.RotatedBy(piOver10 * offsetAmt);

                if (againstWall)
                    offset -= baseOffset;

                int thisArrowType = type;
            if (CalamityInheritanceConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                        if (usPlayer.exoMechLore)
                        {
                            thisArrowType = Main.rand.Next(ExoArrowsExoLore);
                        }
                        else
                        {
                            thisArrowType = Main.rand.Next(ExoArrows);
                        }
                }
                else
                {
                    dmgMult = NormalArrowDamageMult;
                }
            }
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                    if (usPlayer.exoMechLore)
                    {
                        thisArrowType = Main.rand.Next(ExoArrowsExoLore);
                    }
                    else
                    {
                        thisArrowType = Main.rand.Next(ExoArrows);
                    }
                }

                int proj = Projectile.NewProjectile(spawnSource, source + offset, velocity, thisArrowType, damage, knockback, player.whoAmI);

                if (type != ProjectileID.WoodenArrowFriendly)
                {
                    Main.projectile[proj].noDropItem = true;
                }
            }

            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        => Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/HeavenlyGaleoldGlow").Value);

        public override bool CanConsumeAmmo(Item ammo, Player player)
            {
                if (Main.rand.Next(0, 100) < 66)
                    return false;
                return true;
            }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();

            if (usPlayer.exoMechLore == true)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Ranged.HeavenlyGaleold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversionCIWeapon");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversionCIWeapon", AmmoConversionOn));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<Alluvion>().
                AddIngredient<AstrealDefeat>().
                AddIngredient<FlarewingBow>().
                AddIngredient<Phangasm>().
                AddIngredient<TheBallista>().
                AddIngredient<MiracleMatter>().
                AddTile<CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<Alluvion>().
                AddIngredient<AstrealDefeat>().
                AddIngredient<FlarewingBow>().
                AddIngredient<Phangasm>().
                AddIngredient<TheBallista>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge>().
                Register();
        }
    }
}
