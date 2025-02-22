using CalamityInheritance.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Victide
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientVictideBreastplate : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";

        public override void Load()
        {
        }

        public override void SetStaticDefaults()
        {

            // if (Main.netMode != NetmodeID.Server)
            // {
            //     var equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            //     ArmorIDs.Body.Sets.HidesArms[equipSlot] = true;
            //     ArmorIDs.Body.Sets.HidesTopSkin[equipSlot] = true;
            // }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.defense = 5; //9
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.05f;
            player.GetCritChance<GenericDamageClass>() += 5;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 5;
                player.endurance += 0.1f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(6).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
