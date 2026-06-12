using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            if (AncientAeroSet)
            {
                //获取翅膀
                int wSlot = EquipLoader.GetEquipSlot(Mod, "AncientAeroArmor", EquipType.Wings);

                Player.noFallDmg = true;
                if (Player.equippedWings == null)
                {
                    Player.wingsLogic = wSlot;
                    Player.wingTime = 1;
                    Player.wingTimeMax = 1;
                    Player.equippedWings = Player.armor[1];
                }
            }
            ResetLifeMax();
        }
    }
}
