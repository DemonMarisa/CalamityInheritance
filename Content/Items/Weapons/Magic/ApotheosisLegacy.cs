using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NebulousCore = CalamityMod.Items.Accessories.NebulousCore;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class ApotheosisLegacy : CIMagic, ILocalizedModType
    {
        
        public int NewDamage = CIServerConfig.Instance.ShadowspecBuff? 777 : 377;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<Apotheosis>(false);
        }

        public override void SetDefaults()
        {
            Item.damage = NewDamage;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.width = 30;
            Item.height = 34;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ApothMarkLegacy>();
            Item.shootSpeed = 17f;
            Item.Calamity().devItem = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Magic/ApotheosisGlowLegacy").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SubsumingVortexold>().
                AddIngredient<CosmicDischarge>(1).
                AddRecipeGroup(CIRecipeGroup.Norfleet, 3).
                AddRecipeGroup(CIRecipeGroup.Excelsus, 3).
                AddIngredient<TheObliterator>(3).
                AddIngredient<Deathwind>(3).
                AddIngredient<DeathhailStaff>(3).
                AddIngredient<StaffoftheMechworm>(3).
                AddRecipeGroup(CIRecipeGroup.Eradicator, 6).
                AddIngredient<NebulousCore>(3).
                AddIngredient<AscendantSpiritEssence>(77).
                AddIngredient<CosmiliteBar>(77).
                AddIngredient<ShadowspecBar>(7).
                AddTile<CosmicAnvil>().
                Register();

        }
    }
}
