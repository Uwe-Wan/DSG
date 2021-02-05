using System.Windows.Controls;

namespace DSG.Validation.GenerationProfiles
{
    public interface IGenerationProfileValidator
    {
        ValidationResult ValidateName(string profileName);
    }
}
