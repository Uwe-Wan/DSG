using System.Collections.Generic;
using System.Windows.Controls;

namespace DSG.Validation.GenerationProfiles
{
    public interface IGenerationProfileValidator
    {
        ValidationResult ValidateName(string profileName);

        ValidationResult ValidateNameNoDuplicate(string profileName, IEnumerable<string> existingProfileNames);

        ValidationResult ValidateNameNoDuplicate(string profileName);
    }
}
