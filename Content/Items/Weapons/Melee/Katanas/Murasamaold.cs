using CalamityInheritance.Assets;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Katanas;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Katanas
{
    public class Murasamaold : CIMelee
    {

        public int frameCounter = 0;
        public int frame = 0;
        public static bool IDUnlocked(Player player) => CalamityDownBoss.downedYharon || player.name == "Jetstream Sam" || player.name == "Samuel Rodrigues" || player.name == "Sam";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
            ItemID.Sets.AnimatesAsSoul[Type] = false;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 128;
            Item.width = 56;
            Item.damage = 2001;
            Item.DamageType = GetInstance<TrueMelee>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.knockBack = 6.5f;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ProjectileType<MurasamaSlashold>();
            Item.shootSpeed = 24f;
            Item.rare = RarityType<CatalystViolet>();
        }

        public override bool MeleePrefix() => true;
        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture;

            if (IDUnlocked(Main.LocalPlayer))
            {
                //0 = 6 frames, 8 = 3 frames]
                texture = Request<Texture2D>(Texture).Value;
                Rectangle rec = Main.itemAnimations[Item.type].GetFrame(texture);
                spriteBatch.Draw(texture, position, rec, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }
            else
            {
                texture = CIItemTexture.MurasamaSheathed.Value;
                spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture;

            if (IDUnlocked(Main.LocalPlayer))
            {
                texture = Request<Texture2D>(Texture).Value;
                Rectangle rec = Main.itemAnimations[Item.type].GetFrame(texture);
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, rec, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            else
            {
                texture = CIItemTexture.MurasamaSheathed.Value;
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, null, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] > 0)
                return false;
            return IDUnlocked(player);
        }
    }
}
