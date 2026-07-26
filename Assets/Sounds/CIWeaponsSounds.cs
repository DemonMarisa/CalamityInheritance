using Terraria.Audio;

namespace CalamityInheritance.Assets.Sounds
{
    public partial class CISounds
    {
        public static readonly SoundStyle MurasamaOrganicHit = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaHitOrganic") { Volume = 0.45f };
        public static readonly SoundStyle MurasamaInorganicHit = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaHitInorganic") { Volume = 0.55f };
        public static readonly SoundStyle MurasamaSwing = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaSwing") { Volume = 0.2f };
        public static readonly SoundStyle MurasamaBigSwing = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaBigSwing") { Volume = 0.25f };
        public static readonly SoundStyle AncientShivSounds = new("CalamityInheritance/Assets/Sounds/Weapons/AncientShivSounds/AncientShivProjSpawn") { Volume = 1f };

        public static readonly SoundStyle CrystylCharge = new("CalamityInheritance/Assets/Sounds/Weapons/Misc/CrystylCharge") { Volume = 1f };

        public static readonly SoundStyle SwiftSlice = new("CalamityInheritance/Assets/Sounds/Weapons/Misc/SwiftSlice") { Volume = 1f };

        public static readonly SoundStyle LouderPhantomPhoenix = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/LouderPhantomPhoenix", 3);

        public static readonly SoundStyle ScissorGuillotineSnapSound = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/ScissorGuillotineSnap");
    }
}
