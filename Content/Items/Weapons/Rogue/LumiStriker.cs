using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Sounds.Custom;
using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class LumiStriker: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 86;
            Item.width = 102;
            Item.damage = 180;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red; 
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 16f;
            Item.shoot = ModContent.ProjectileType<LumiStrikerProj>();
            Item.shootSpeed = 22f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool stealth = player.Calamity().StealthStrikeAvailable();
            int p = Projectile.NewProjectile(source, position, velocity * 1.2f, type, damage, knockback, player.whoAmI);
            
            if (!stealth)
                return false;
                
            SoundEngine.PlaySound(CISoundMenu.HammerReturnID1 with {Volume = 0.7f, Pitch = 0.5f});
            Main.projectile[p].Calamity().stealthStrike = stealth;
            Main.projectile[p].damage = (int)(damage * 0.70f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpearofPaleolith>().
                AddIngredient<Turbulance>().
                AddIngredient(ItemID.FragmentStardust, 6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}