using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.RogueMelee;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.RogueMelee
{
    public class GalaxySmasher : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void ExSD()
        {
            EffectColor = Color.Violet;

            Item.width = 86;
            Item.height = 72;
            Item.DamageType = RogueDamage.Instance;
            Item.damage = 300;
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;

            Item.shoot = ProjectileType<GalaxySmasherProj>();
            Item.shootSpeed = 20f;

            Item.LAP().SkillShoot = ProjectileType<GalaxySmasherProj>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(Item.UseSound);
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 1f);
            Main.projectile[p].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateCalRecipe(Type).
                AddCalIngredient<StellarContempt>().
                AddCalIngredient(CalamityMaterials.CosmiliteBar, 10).
                AddCalTile(CalamityTile.CosmicAnvilTile).
                RegisterCal();
            if (!HasCalamity())
                return;
            CreateRecipe().
                AddIngredient(ItemType<StellarContempt>()).
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
