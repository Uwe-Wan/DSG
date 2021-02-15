using System.Windows.Controls;

namespace DSG.Validation.Propabilities
{
    public interface IPropabilityValidator
    {
        ValidationResult ValidatePropability(int value);
    }
}
