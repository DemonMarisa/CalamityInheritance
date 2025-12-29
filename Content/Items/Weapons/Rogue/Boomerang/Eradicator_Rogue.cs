using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Melee.Boomerang;
using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityInheritance.Content.Projectiles.Typeless.NorProj;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Particles;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class Eradicator_Rogue : RogueWeapon, ILocalizedModType
    {
        public static float Speed = 9.0f;
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 58;
            Item.damage = 100;
            //降低飞盘伤害，提高星云射线的倍率（0.4→0.8），且极大程度地提高了星云射线的索敌范围与蛇毒，与发射频率
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.knockBack = 7f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.shoot = ProjectileType<EradicatorProj_Rogue>();
            Item.shootSpeed = Speed;
            Item.DamageType = GetInstance<RogueDamageClass>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CosmiliteBar>(12)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
