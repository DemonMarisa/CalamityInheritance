﻿using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Victide
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientVictideLeggings: CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {

            if (Main.netMode != NetmodeID.Server)
            {
                var equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
                ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlot] = true;
            }
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.defense = 4; //9
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += Collision.DrownCollision(player.position, player.width, player.height, player.gravDir) ? 0.3f : 0.08f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(2).
                AddIngredient<PearlShard>(2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
