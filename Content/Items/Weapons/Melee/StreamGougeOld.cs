using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee.Spears;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Melee.Spear;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Weapons.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class StreamGougeOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static float ProjShootSpeed = 20f;
        public static int FadeoutSpeed = 20;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Item.type] = true;
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //关闭微光转化后，利维坦龙涎香正常掉落
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<StreamGouge>()] = ModContent.ItemType<StreamGougeOld>();
            }
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.damage = 350;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 19;
            Item.knockBack = 9.75f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.height = 100;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shoot = ModContent.ProjectileType<StreamGougeProjOld>();
            Item.shootSpeed = 12f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 origin = new Vector2(50f, 48f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/StreamGougeOldGlow").Value, Item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == false) //关闭微光转化后，利维坦龙涎香正常掉落
            {
                Recipe recipe = CreateRecipe();
                recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 14);
                recipe.AddTile(ModContent.TileType<CosmicAnvil>());
                recipe.Register();
            }
        }
    }
}
