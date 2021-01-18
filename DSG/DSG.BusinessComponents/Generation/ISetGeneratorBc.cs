using DSG.BusinessEntities;

namespace DSG.BusinessComponents.Generation
{
    public interface ISetGeneratorBc
    {
        GeneratedSetDto GenerateSet(GenerationParameterDto generationParameter);
    }
}
