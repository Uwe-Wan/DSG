using System.Windows.Controls.Primitives;

namespace DSG.WinUI.Behavior
{
    public class DropDownButtonToTopBehavior : DropDownButtonBehavior
    {
        protected override PlacementMode ChosenPlacement => PlacementMode.Top;
    }
}
