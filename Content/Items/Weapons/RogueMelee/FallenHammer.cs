using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.RogueMelee;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.RogueMelee
{
    public class FallenHammer : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void ExSD()
        {
            EffectColor = Color.Red;

            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = RogueDamage.Instance;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<FallenHammerProj>();
            Item.shootSpeed = 14f;

            Item.LAP().SkillShoot = ProjectileType<FallenHammerProj>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(Item.UseSound);
            int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 1f);
            Main.projectile[p].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateCalRecipe(Type).
                AddCalIngredient(ItemID.PaladinsHammer).
                AddCalIngredient<Pwnagehammer>().
                AddCalIngredient(CalamityMaterials.ScoriaBar, 5).
                AddCalTile(TileID.MythrilAnvil).
                RegisterCal();
            if (!HasCalamity())
                return;
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<Pwnagehammer>().
                AddIngredient(ItemID.BeetleHusk, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
