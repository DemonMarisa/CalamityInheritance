using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeHammerPwnageLegacy: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory =>"Content.Items.Weapons.Rogue";
        public static readonly float Speed = 12f;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = 90;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 10f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>();
            Item.shootSpeed = 12f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Pwnhammer).
                AddIngredient(ItemID.HallowedBar, 7).
                AddIngredient(ItemID.SoulofFright, 3).
                AddIngredient(ItemID.SoulofMight, 3).
                AddIngredient(ItemID.SoulofSight, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}