using CalamityInheritance.Tiles.Vanity;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityInheritance.World;
using CalamityInheritance.CIPlayer;
using CalamityMod.World;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class Malice : CIPlaceable, ILocalizedModType
    {
        public int frameCounter = 0;
        public int frame = 0;
        public new string LocalizationCategory => $"{Local}.Vanity";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 66;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
            Item.createTile = ModContent.TileType<MaliceTiles>();
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.vanity = true;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (CalamityUtils.AnyBossNPCS())
                return false;
            if (player.altFunctionUse == 2)
            {
                Item.useAnimation = 45;
                Item.useTime = 45;
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.UseSound = SoundID.Item119;
            }
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!CIWorld.Malice)
                {
                    if (!CalamityWorld.revenge)
                    {
                        CalamityWorld.revenge = true;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.RevengeText", Color.Crimson);
                    }
                    if (!CalamityWorld.death)
                    {
                        CalamityWorld.death = true;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DeathText", Color.Crimson);
                    }
                    CIWorld.Malice = true;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText", Color.LightGoldenrodYellow);
                }
                else
                {
                    CIWorld.Malice = false;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText2", Color.LightGoldenrodYellow);
                }
                CalamityNetcode.SyncWorld();
            }
            return true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Placeables/Vanity/Malice_Animated").Value;
            spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, 8, 8), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Placeables/Vanity/Malice_Animated").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 8, 8), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
