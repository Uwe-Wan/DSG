using System.Windows.Controls.Primitives;

namespace DSG.WinUI.Behavior
{
    public class DropDownButtonToRightBehavior : DropDownButtonBehavior
    {
        protected override PlacementMode ChosenPlacement => PlacementMode.Right;
    }
}
