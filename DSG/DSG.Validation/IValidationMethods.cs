using System.Collections.Generic;
using System.Windows.Controls;

namespace DSG.Validation
{
    public interface IValidationMethods
    {
        ValidationResult ValidateStringLength(string name, int length);

        ValidationResult ValidateStringNotNullNotEmpty(string name);

        ValidationResult ValidateNameNoDuplicate(string name, IEnumerable<string> existingNames, string propertyName);
    }
}
