using DSG.Common.Exceptions;
using log4net;

namespace DSG.Common
{
    public static class Check
    {
        private static ILog Logger => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void RequireInjected(object requiredObject, string objectName, string nameOfParentClass)
        {
            if (requiredObject == null)
            {
                string errorMessage = $"The {objectName} was not correctly injected into {nameOfParentClass}.";

                Logger.Error(errorMessage);
                throw new TechnicalException(errorMessage);
            }
        }

        public static void RequireNotNull(object requiredObject, string objectName, string nameOfParentClass)
        {
            if (requiredObject == null)
            {
                string errorMessage = $"The {objectName} in {nameOfParentClass} is null but must not be.";

                Logger.Error(errorMessage);
                throw new TechnicalException(errorMessage);
            }
        }

        public static void RequireNotNullNotEmpty(string requiredObject, string objectName, string nameOfParentClass)
        {
            RequireNotNull(requiredObject, objectName, nameOfParentClass);

            if (requiredObject == string.Empty)
            {
                string errorMessage = $"The {objectName} in {nameOfParentClass} is empty but must not be.";

                Logger.Error(errorMessage);
                throw new TechnicalException(errorMessage);
            }
        }
    }
}
