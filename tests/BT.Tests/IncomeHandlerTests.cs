using BT.APPLICATION;
using BT.APPLICATION.BudgetTracker.Income.Models;
using BT.APPLICATION.RequestResponse;
using BT.DOMAIN.Enums.BudgetTracker;
using Microsoft.EntityFrameworkCore;
using Create = BT.APPLICATION.BudgetTracker.Income.Command.Create;
using GetList = BT.APPLICATION.BudgetTracker.Income.Queries.GetList;
using Update = BT.APPLICATION.BudgetTracker.Income.Command.Update;

namespace BT.Tests;

public class CreateIncomeTests
{
    private static Create.Command SampleCommand() => new()
    {
        Date = new DateTime(2025, 1, 15),
        Banks = [new BankAccountModel { Name = "BDO", Balance = 5000 }],
        Cash = [new CashOnHandModel { Denomination = Denominations.ONE_HUNDRED, Pieces = 3 }],
        Deductions = [new DeductionModel { Description = "Rent", Amount = 1200 }],
    };

    [Fact]
    public async Task Create_PersistsIncomeWithItsBanksCashAndDeductions()
    {
        var db = new TestDb();

        var response = await new Create.CommandHandler(db.NewContext(), TestDb.Mapper)
            .Handle(SampleCommand(), CancellationToken.None);

        var success = Assert.IsType<SuccessResponse<IncomeModel>>(response);
        Assert.Equal("BDO", Assert.Single(success.Data.Banks).Name);

        await using var verify = db.NewContext();
        var saved = await verify.Incomes
            .Include(x => x.Banks).Include(x => x.Cash).Include(x => x.Deductions)
            .SingleAsync();
        Assert.Equal(new DateTime(2025, 1, 15), saved.Date);
        Assert.Equal(5000, Assert.Single(saved.Banks).Balance);
        Assert.Equal(3, Assert.Single(saved.Cash).Pieces);
        Assert.Equal("Rent", Assert.Single(saved.Deductions).Description);
    }

    [Fact]
    public async Task Create_StampsAuditFields_FromTheSignedInUser()
    {
        var db = new TestDb(userId: "user-1", email: "gelo@example.com");

        await new Create.CommandHandler(db.NewContext(), TestDb.Mapper)
            .Handle(SampleCommand(), CancellationToken.None);

        await using var verify = db.NewContext();
        var saved = await verify.Incomes.SingleAsync();
        Assert.Equal("user-1", saved.CreatedById);
        Assert.Equal("gelo@example.com", saved.CreatedBy);
    }

    [Fact]
    public async Task Create_FallsBackToNotAvailable_WhenNobodyIsSignedIn()
    {
        var db = new TestDb();

        await new Create.CommandHandler(db.NewContext(), TestDb.Mapper)
            .Handle(SampleCommand(), CancellationToken.None);

        await using var verify = db.NewContext();
        var saved = await verify.Incomes.SingleAsync();
        Assert.Equal("n/a", saved.CreatedById);
        Assert.Equal("n/a", saved.CreatedBy);
    }
}

public class GetIncomeListTests
{
    private static async Task SeedAsync(TestDb db, int count)
    {
        for (var i = 1; i <= count; i++)
        {
            await new Create.CommandHandler(db.NewContext(), TestDb.Mapper).Handle(
                new Create.Command { Date = new DateTime(2025, 1, i) },
                CancellationToken.None);
        }
    }

    [Fact]
    public async Task GetList_ReturnsRequestedPage_WithPagingMetadata()
    {
        var db = new TestDb();
        await SeedAsync(db, 5);

        var response = await new GetList.QueryHandler(db.NewContext(), TestDb.Mapper)
            .Handle(new GetList.Query { PageNumber = 2, PageSize = 2 }, CancellationToken.None);

        var page = Assert.IsType<SuccessResponse<PaginatedResponse<List<IncomeModel>>>>(response).Data;
        Assert.Equal(2, page.Data.Count);
        Assert.Equal(2, page.PageNumber);
        Assert.Equal(5, page.TotalCount);
        Assert.Equal(3, page.TotalPages);
    }

    [Fact]
    public async Task GetList_LastPageHoldsTheRemainder()
    {
        var db = new TestDb();
        await SeedAsync(db, 5);

        var response = await new GetList.QueryHandler(db.NewContext(), TestDb.Mapper)
            .Handle(new GetList.Query { PageNumber = 3, PageSize = 2 }, CancellationToken.None);

        var page = Assert.IsType<SuccessResponse<PaginatedResponse<List<IncomeModel>>>>(response).Data;
        Assert.Single(page.Data);
    }

    [Fact]
    public async Task GetList_ReturnsEmptyPage_WhenThereAreNoRecords()
    {
        var response = await new GetList.QueryHandler(new TestDb().NewContext(), TestDb.Mapper)
            .Handle(new GetList.Query(), CancellationToken.None);

        var page = Assert.IsType<SuccessResponse<PaginatedResponse<List<IncomeModel>>>>(response).Data;
        Assert.Empty(page.Data);
        Assert.Equal(0, page.TotalCount);
    }
}

public class UpdateIncomeTests
{
    private static async Task<int> SeedIncomeAsync(TestDb db)
    {
        var created = await new Create.CommandHandler(db.NewContext(), TestDb.Mapper).Handle(
            new Create.Command
            {
                Date = new DateTime(2025, 1, 15),
                Banks = [new BankAccountModel { Name = "BDO", Balance = 5000 }],
                Cash = [new CashOnHandModel { Denomination = Denominations.ONE_HUNDRED, Pieces = 3 }],
                Deductions = [new DeductionModel { Description = "Rent", Amount = 1200 }],
            },
            CancellationToken.None);

        return Assert.IsType<SuccessResponse<IncomeModel>>(created).Data.Id;
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenTheIncomeDoesNotExist()
    {
        var response = await new Update.CommandHandler(new TestDb().NewContext(), TestDb.Mapper)
            .Handle(new Update.Command { Income = new IncomeModel { Id = 42 } }, CancellationToken.None);

        var bad = Assert.IsType<BadRequestResponse>(response);
        Assert.Equal("Cannot find item.", bad.Message);
    }

    [Fact]
    public async Task Update_PersistsTheNewDateAndReplacesTheChildRows()
    {
        var db = new TestDb();
        var id = await SeedIncomeAsync(db);

        var response = await new Update.CommandHandler(db.NewContext(), TestDb.Mapper).Handle(
            new Update.Command
            {
                Income = new IncomeModel
                {
                    Id = id,
                    Date = new DateTime(2025, 2, 1),
                    Banks = [new BankAccountModel { Name = "BPI", Balance = 7500 }],
                    Cash = [],
                    Deductions =
                    [
                        new DeductionModel { Description = "Internet", Amount = 1500 },
                        new DeductionModel { Description = "Power", Amount = 2000 },
                    ],
                },
            },
            CancellationToken.None);

        Assert.IsType<SuccessResponse<IncomeModel>>(response);

        await using var verify = db.NewContext();
        var saved = await verify.Incomes
            .Include(x => x.Banks).Include(x => x.Cash).Include(x => x.Deductions)
            .SingleAsync();
        Assert.Equal(new DateTime(2025, 2, 1), saved.Date);
        Assert.Equal("BPI", Assert.Single(saved.Banks).Name);
        Assert.Empty(saved.Cash);
        Assert.Equal(2, saved.Deductions.Count);
        Assert.Equal(1, await verify.BankAccounts.CountAsync());   // the old "BDO" row is gone, not orphaned
        Assert.Equal(0, await verify.CashOnHands.CountAsync());
    }

    [Fact]
    public async Task Update_StampsLastUpdatedBy_WithTheSignedInUser()
    {
        var db = new TestDb(userId: "user-7", email: "gelo@example.com");
        var id = await SeedIncomeAsync(db);

        await new Update.CommandHandler(db.NewContext(), TestDb.Mapper).Handle(
            new Update.Command { Income = new IncomeModel { Id = id, Date = new DateTime(2025, 3, 1) } },
            CancellationToken.None);

        await using var verify = db.NewContext();
        var saved = await verify.Incomes.SingleAsync();
        Assert.Equal("user-7", saved.LastUpdatedById);
        Assert.Equal("gelo@example.com", saved.LastUpdatedBy);
        Assert.Equal(1, saved.Version);
    }

    [Fact]
    public async Task Update_FallsBackToNotAvailable_WhenNobodyIsSignedIn()
    {
        var db = new TestDb();
        var id = await SeedIncomeAsync(db);

        await new Update.CommandHandler(db.NewContext(), TestDb.Mapper).Handle(
            new Update.Command { Income = new IncomeModel { Id = id, Date = new DateTime(2025, 3, 1) } },
            CancellationToken.None);

        await using var verify = db.NewContext();
        Assert.Equal("n/a", (await verify.Incomes.SingleAsync()).LastUpdatedById);
    }
}

public class PaginatedResponseTests
{
    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(1, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(25, 10, 3)]
    public void TotalPages_RoundsUp(int totalCount, int pageSize, int expectedPages)
    {
        var page = new PaginatedResponse<List<int>>([], 1, pageSize, totalCount);

        Assert.Equal(expectedPages, page.TotalPages);
    }
}
