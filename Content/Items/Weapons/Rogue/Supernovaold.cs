using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using System.Dynamic;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Supernovaold : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public static readonly SoundStyle ExplosionSound = new("CalamityMod/Sounds/Item/SupernovaBoom") { Volume = 0.8f };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;
            Item.damage = 2100;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ModContent.ProjectileType<SupernovaBombold>();
            Item.shootSpeed = 19f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override float StealthDamageMultiplier => 1.20f;
        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //右键的逻辑会比较奇怪。
            if (player.altFunctionUse == 2 && (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo) && player.ownedProjectileCounts[ModContent.ProjectileType<SupernovaBombold>()] > 0)
            {
                //搜寻场上所有射弹
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    //符合条件，我们直接提前引爆这个射弹
                    if (proj.friendly && proj.owner == player.whoAmI && proj.DamageType == ModContent.GetInstance<RogueDamageClass>() && proj.type == ModContent.ProjectileType<SupernovaBombold>())
                    {
                        proj.damage = (int)(damage * 1.2f);
                        proj.CalamityInheritance().GlobalRightClickListener = true;
                        proj.netUpdate = true;
                    }
                }
                SoundStyle getSound = new ("CalamityMod/Sounds/Custom/ExoMechs/ApolloArtemisTargetSelection");
                SoundEngine.PlaySound(getSound, player.Center);
            }
            else if (player.altFunctionUse != 2)
            {
                int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[p].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Rogue/SupernovaoldGlow").Value);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Rogue.Supernovaold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SealedSingularity>().
                AddIngredient<StarofDestruction>().
                AddIngredient<TotalityBreakers>().
                AddIngredient<BallisticPoisonBomb>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
            
            CreateRecipe().
                AddIngredient<SealedSingularity>().
                AddIngredient<StarofDestruction>().
                AddIngredient<TotalityBreakers>().
                AddIngredient<BallisticPoisonBomb>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
