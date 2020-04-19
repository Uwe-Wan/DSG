using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using DSG.Common.Extensions;
using FluentAssertions;

namespace DSG.Common.Test.Extensions
{
    [TestFixture]
    public class ObservableCollectionExtensionTest
    {
        [Test]
        public void AddRange_RangeOfTwo_IsAdded()
        {
            //Arrange
            ObservableCollection<int> collection = new ObservableCollection<int>();
            List<int> list = new List<int> { 1, 2 };

            //Act
            collection.AddRange(list);

            //Assert
            collection.Should().HaveCount(2);
        }
    }
}
