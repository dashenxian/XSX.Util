using Shouldly;
using System.Reflection;
using System.Threading.Tasks;
using XSX.Threading;
using Xunit;

namespace Tests.XSX.Threading;

public class AsyncHelperTest
{
    private bool _executeGetVoidAsyncStatusCompleted;
    private Task<int> GetIntAsync()
    {
        return Task.FromResult(1);
    }
    private async Task GetVoidAsync()
    {
        await Task.Delay(10);
        _executeGetVoidAsyncStatusCompleted = true;
    }
    private int GetInt()
    {
        return 1;
    }
    [Fact]
    public void IsAsyncMethodTest()
    {
        typeof(AsyncHelperTest).GetMethod(nameof(GetInt), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.IsAsyncMethod().ShouldBe(false);
        typeof(AsyncHelperTest).GetMethod(nameof(GetIntAsync), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.IsAsyncMethod().ShouldBe(true);
    }
    [Fact]
    public void IsTaskOrTaskOfTTest()
    {
        Task.FromResult(1).GetType().IsTaskOrTaskOfT().ShouldBe(true);
        typeof(Task).IsTaskOrTaskOfT().ShouldBe(true);
        1.GetType().IsTaskOrTaskOfT().ShouldBe(false);
    }
    [Fact]
    public void IsTaskOfTTest()
    {
        Task.FromResult(1).GetType().IsTaskOfT().ShouldBe(true);
        typeof(Task).IsTaskOfT().ShouldBe(false);
    }

    [Fact]
    public void UnwrapTaskTest()
    {
        AsyncHelper.UnwrapTask(Task.FromResult(new object()).GetType()).ShouldBe(typeof(object));
        AsyncHelper.UnwrapTask(Task.FromResult(1).GetType()).ShouldBe(typeof(int));
        AsyncHelper.UnwrapTask(typeof(Task)).ShouldBe(typeof(void));
    }

    [Fact]
    public void RunSyncTResultTest()
    {
        AsyncHelper.RunSync(GetIntAsync).ShouldBe(1);
    }
    [Fact]
    public void RunSyncTest()
    {
        _executeGetVoidAsyncStatusCompleted = false;
        AsyncHelper.RunSync(GetVoidAsync);
        _executeGetVoidAsyncStatusCompleted.ShouldBe(true);
    }
    [Fact]
    public void RunNotSyncTest()
    {
        _executeGetVoidAsyncStatusCompleted = false;
        _ = GetVoidAsync();
        _executeGetVoidAsyncStatusCompleted.ShouldBe(false);
    }
}