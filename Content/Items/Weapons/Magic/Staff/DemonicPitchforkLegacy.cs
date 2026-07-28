using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Magic.Staff;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Staff
{
    public class DemonicPitchforkLegacy : CIMagic
    {

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 82;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 11;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = CISoundID.SoundCurseFlamesAttack;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DemonicPitchforkProjLegacy>();
            Item.shootSpeed = 16f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Obsidian, 20).
                AddIngredient(ItemID.HellstoneBar, 20).
                AddIngredient(ItemID.SoulofNight, 20).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
