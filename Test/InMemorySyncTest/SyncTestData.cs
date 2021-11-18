using System.Collections.Generic;
using InMemorySyncTest.Model;

namespace InMemorySyncTest;

public partial class InMemorySyncTest
{
    internal class SyncTestData
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[]
                {
                    new object[]
                    {
                        new Mock(1),
                        new Mock(2),
                        new Mock(3),
                        new Mock(4),
                    }
                }
            };
    }
}