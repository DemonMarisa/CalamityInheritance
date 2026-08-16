using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class UnstableGraniteCoreLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().UnstableGraniteCore = true;
        }
        public static void OnHitNPC_UGC(CIPlayer player,NPC target, NPC.HitInfo hit, int damageDone)
        {
            //花岗岩核心
            if (!player.HasCount("UnstableGraniteCoreSparkTimer") && hit.Crit)
            {
                int sparksNum = 6;
                for (int i = 0; i < sparksNum; i++)
                {
                    //这里是为了绕过0的处理
                    Vector2 sparkSpeed = Main.rand.NextBool(2) ? new(Main.rand.NextFloat(-50f, 0f), Main.rand.NextFloat(-50f, 0f)) : new(Main.rand.NextFloat(1f, 51f), Main.rand.NextFloat(0f, 51f));
                    sparkSpeed.Normalize();
                    sparkSpeed *= Main.rand.NextFloat(30f, 61f) * 0.1f;
                    Projectile.NewProjectile(player.Player.GetSource_FromThis(), target.Center.X, target.Center.Y, sparkSpeed.X, sparkSpeed.Y, ProjectileType<ElectricSpark>(), (int)(hit.Damage * 0.15f), 0f, player.Player.whoAmI, 0f, 0f);
                }
                player.AddCount("UnstableGraniteCoreSparkTimer", 10);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Granite, 15);
            recipe.Register();
        }
    }
}