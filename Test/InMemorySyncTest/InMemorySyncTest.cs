using InMemorySyncTest.Model;
using Microsoft.EntityFrameworkCore;
using SyncSharp;
using SyncSharp.Commons.DbSource;
using Xunit;

namespace InMemorySyncTest;

public partial class InMemorySyncTest
{
    [Theory]
    [MemberData(nameof(SyncTestData.TestData), MemberType = typeof(SyncTestData))]
    public void SyncTest(object[] testSyncData)
    {
        var syncHelper = new SyncHelper<InMemoryTestDbContext>(
            new InMemoryDbSource
            {
                DatabaseName = "sender",
                Seed = testSyncData
            },
            new InMemoryDbSource
            {
                DatabaseName = "receiver"
            });
        syncHelper.AddSync<Mock>();

        using var senderContext = new InMemoryTestDbContext(
            new DbContextOptionsBuilder<InMemoryTestDbContext>()
                .UseInMemoryDatabase("sender")
                .Options);

        using var receiverContext = new InMemoryTestDbContext(
            new DbContextOptionsBuilder<InMemoryTestDbContext>()
                .UseInMemoryDatabase("receiver")
                .Options);

        syncHelper.StartSync(senderContext, receiverContext);

        Assert.Equal(senderContext.Mock, receiverContext.Mock);
    }
}