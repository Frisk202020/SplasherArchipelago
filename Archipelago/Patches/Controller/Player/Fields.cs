using System.Reflection;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Player {
    internal static class Fields {
        internal static readonly FieldInfo groundState = AccessTools.DeclaredField(typeof(PlayerController), "paint_Ground");
        internal static readonly FieldInfo groundCollider = AccessTools.DeclaredField(typeof(PlayerController), "groundCollider");
        internal static readonly MethodInfo trail = AccessTools.DeclaredPropertySetter(typeof(PlayerController), "CurrenTrailAnchor");
        internal static readonly FieldInfo freeze = AccessTools.DeclaredField(typeof(PlayerController), "positionFreeze");
        internal static readonly FieldInfo autoStickCorner = AccessTools.DeclaredField(typeof(PlayerController), "autoStickCorner");
        internal static readonly FieldInfo velocity = AccessTools.DeclaredField(typeof(PlayerController), "_velocity");
        internal static readonly FieldInfo leftStickSign = AccessTools.DeclaredField(typeof(PlayerController), "leftJoystickSign");
        internal static readonly FieldInfo leftStickAxis = AccessTools.DeclaredField(typeof(PlayerController), "leftJoystickAxis");
        internal static readonly FieldInfo bounceControl = AccessTools.DeclaredField(typeof(PlayerController), "bounceControl");
    }
}
