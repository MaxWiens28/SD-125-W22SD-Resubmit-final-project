using Moq;
using SD_340_W22SD_2021_2022___Final_Project_2.BLL;
using SD_340_W22SD_2021_2022___Final_Project_2.DAL;
using SD_340_W22SD_2021_2022___Final_Project_2.Models;

namespace UnitTesting;

[TestClass]
public class UnitTest1
{
    private UserBusinessLogic userBL;
    private List<ApplicationUser> allUsers;
    private ApplicationUser user;

    [TestInitialize]
    public void Initialize()
    {
        ApplicationUser mockUser1 = new ApplicationUser() { Id = "1", UserName = "a" };
        ApplicationUser mockUser2 = new ApplicationUser() { Id = "2", UserName = "b" };
        ApplicationUser mockUser3 = new ApplicationUser() { Id = "3", UserName = "c" };

        allUsers = new List<ApplicationUser>() { mockUser1, mockUser1, mockUser1, };

        Mock<IRepository<ApplicationUser>> mockRepo = new Mock<IRepository<ApplicationUser>>();

        mockRepo.Setup(repo => repo.Get(It.IsAny<int>())).Returns<string>((str) => allUsers.First(user => user.Id == str));

        mockRepo.Setup(repo => repo.Get(It.IsAny<int>())).Returns<string>((username) => allUsers.First(user => user.UserName == username));
        mockRepo.Setup(repo => repo.GetList(It.IsAny<Func<ApplicationUser, bool>>())).Returns<Func<ApplicationUser, bool>>((func) => allUsers.Where(func).ToList());


        userBL = new UserBusinessLogic(mockRepo.Object);
    }

    [TestMethod]
    public void GetUserTest()
    {
        Assert.AreSame(allUsers.First(u => u.Id == "1"), userBL.GetUser("1"));
        Assert.AreSame(allUsers.First(u => u.Id == "3"), userBL.GetUser("3"));
    }

    [TestMethod]
    public void GetUserTest_Failed()
    {
        Assert.AreNotSame(allUsers.First(u => u.Id == "1"), userBL.GetUser("2"));
        Assert.AreNotSame(allUsers.First(u => u.Id == "3"), userBL.GetUser("1"));
    }

    [TestMethod]
    public void GetUserByUsernameTest()
    {
        Assert.AreSame(allUsers.First(u => u.UserName == "a"), userBL.GetUser("a"));
        Assert.AreSame(allUsers.First(u => u.UserName == "b"), userBL.GetUser("b"));
    }

    [TestMethod]
    public void GetUserByUsernameTest_Failed()
    {
        Assert.AreNotSame(allUsers.First(u => u.UserName == "a"), userBL.GetUser("b"));
        Assert.AreNotSame(allUsers.First(u => u.UserName == "b"), userBL.GetUser("c"));
    }

    [TestMethod]
    public void GetUsersWhoAreNotTheTicketOwnerTest()
    {
        var user1 = allUsers[0];
        var user2 = allUsers[1];
        var user3 = allUsers[2];

        var whereList = userBL.GetUsersWhoAreNotTheTicketOwner(user1);

        CollectionAssert.Contains(whereList, user1);
        CollectionAssert.Contains(whereList, user2);
        CollectionAssert.DoesNotContain(whereList, user3);
    }
}

