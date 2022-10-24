using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using SD_340_W22SD_2021_2022___Final_Project_2.BLL;
using SD_340_W22SD_2021_2022___Final_Project_2.Models;
using SD_340_W22SD_2021_2022___Final_Project_2.Data;
using SD_340_W22SD_2021_2022___Final_Project_2.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

//HELLO
namespace SD_340_W22SD_2021_2022___Final_Project_2_UnitTests
{
    [TestClass]
    public class BLUnitTest
    {
        // hello world
        private UserBusinessLogic UserBL;
        private TicketBusinessLogic TicketBL;
        private CommentBusinessLogic CommentBL;
        private ProjectBusinessLogic ProjectBL;

        public readonly UserManager<ApplicationUser> UserManager;

        public class FakeUserManager : UserManager<ApplicationUser>
        {
            public FakeUserManager()
                : base(new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
            {
            }
        }
        public BLUnitTest()
        {
            var userdata = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "M0ckUser1!",
                    Email = "User1@Mockdata.ca",
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "M0ckUser2!",
                    Email = "User2@Mockdata.ca",
                },
                new ApplicationUser
                {
                    Id = "3",
                    UserName = "M0ckUser3!",
                    Email = "User3@Mockdata.ca",
                }
            }.AsQueryable();

            var userMockDbSet = new Mock<FakeUserManager>();

            userMockDbSet.Setup(u => u.Users).Returns(userdata);
            userMockDbSet.Setup(c => c.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userMockDbSet.Setup(u => u.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            userMockDbSet.Setup(fbi => fbi.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) => UserManager.Users.SingleOrDefault(u => u.Id == userId));
            userMockDbSet.Setup(fbn => fbn.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((string userName) => UserManager.Users.SingleOrDefault(u => u.Id == userName));
            userMockDbSet.Setup(r => r.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "Admin", "Project Manager", "Developer" });
            userMockDbSet.Setup(ar => ar.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userMockDbSet.Setup(ur => ur.GetUsersInRoleAsync(It.IsAny<string>())).ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { UserName = "Developer" } });
            UserManager = userMockDbSet.Object;

            UserBL = new UserBusinessLogic(UserManager);

            var projectData = new List<Project>
            {
                new Project{Id = 1, Name = "Project1", Developers = UserManager.Users.ToList()},
                new Project{Id = 2, Name = "Project2", Developers = UserManager.Users.ToList()},
                new Project{Id = 3, Name = "Project3", Developers = UserManager.Users.ToList()},
                new Project{Id = 4, Name = "Project4", Developers = UserManager.Users.ToList()}
            }.AsQueryable();

            var projectMockDbSet = new Mock<DbSet<Project>>();

            projectMockDbSet.As<IQueryable<Project>>().Setup(m => m.Provider).Returns(projectData.Provider);
            projectMockDbSet.As<IQueryable<Project>>().Setup(m => m.Expression).Returns(projectData.Expression);
            projectMockDbSet.As<IQueryable<Project>>().Setup(m => m.ElementType).Returns(projectData.ElementType);
            projectMockDbSet.As<IQueryable<Project>>().Setup(m => m.GetEnumerator()).Returns(projectData.GetEnumerator());

            var projectMockContent = new Mock<ApplicationDbContext>();
            projectMockContent.Setup(c => c.Project).Returns(projectMockDbSet.Object);

            ProjectBL = new ProjectBusinessLogic(new ProjectRepository(projectMockContent.Object), UserManager);

            var ticketData = new List<Ticket>
            {
                new Ticket{Id = 1, Name = "Ticket1", Hours = 10, Completed = false, Priority = Priority.low, ProjectId = 1, Developers = UserManager.Users.ToList(), TaskOwners = (ICollection<ApplicationUser>)userdata.Take(1), TaskWatchers = (ICollection<ApplicationUser>)userdata.Take(3)},
                new Ticket{Id = 2, Name = "Ticket2", Hours = 20, Completed = true, Priority = Priority.high, ProjectId = 2, Developers = UserManager.Users.ToList(), TaskOwners = (ICollection<ApplicationUser>)userdata.Take(2), TaskWatchers = (ICollection<ApplicationUser>)userdata.Take(1)},
                new Ticket{Id = 3, Name = "Ticket3", Hours = 30, Completed = false, Priority = Priority.medium, ProjectId = 3, Developers = UserManager.Users.ToList(), TaskOwners = (ICollection<ApplicationUser>)userdata.Take(3), TaskWatchers = (ICollection<ApplicationUser>)userdata.Take(2)}
            }.AsQueryable();

            var ticketMockDbSet = new Mock<DbSet<Ticket>>();

            ticketMockDbSet.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(ticketData.Provider);
            ticketMockDbSet.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(ticketData.Expression);
            ticketMockDbSet.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(ticketData.ElementType);
            ticketMockDbSet.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(ticketData.GetEnumerator());

            var ticketMockContent = new Mock<ApplicationDbContext>();
            ticketMockContent.Setup(c => c.Ticket).Returns(ticketMockDbSet.Object);

            TicketBL = new TicketBusinessLogic(new TicketRepository(ticketMockContent.Object));

            var commentData = new List<Comment>
            {
                new Comment{Id = 1, Content = "Hello", TicketId = 1, UserId = "1"},
                new Comment{Id = 2, Content = "Hi", TicketId = 1, UserId = "2"},
                new Comment{Id = 3, Content = "Hey", TicketId = 2, UserId = "3"},
                new Comment{Id = 4, Content = "How are you", TicketId = 3, UserId = "2"}
            }.AsQueryable();

            var commentMockDbSet = new Mock<DbSet<Comment>>();

            commentMockDbSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(commentData.Provider);
            commentMockDbSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(commentData.Expression);
            commentMockDbSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(commentData.ElementType);
            commentMockDbSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(commentData.GetEnumerator());

            var commentMockContent = new Mock<ApplicationDbContext>();
            commentMockContent.Setup(c => c.Comment).Returns(commentMockDbSet.Object);

            CommentBL = new CommentBusinessLogic(new CommentRepository(commentMockContent.Object));
        }
        [DataRow(1, 2)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [TestMethod]
        public void GetAllCommentsByTicket_validInput_ResturnsAllComment(int ticketId, int ticketCount)
        {
            List<Comment> comments = CommentBL.GetAllCommentsByTicket(ticketId);

            Assert.AreEqual(ticketCount, comments.Count);
        }

        [TestMethod]
        public void GetAllCommentsByTicket_TicketNotFound_ThrowsException()
        {
            Assert.ThrowsException < KeyNotFoundException>(() => CommentBL.GetAllCommentsByTicket(99));
        }

        [DataRow(1, "Ticket1")]
        [DataRow(2, "Ticket2")]
        [DataRow(3, "Ticket3")]
        [TestMethod]
        public void FindTicketById_validInput_ReturnATicket(int TicketId, string TicketName)
        {
            Ticket ticket = TicketBL.FindTicketById(TicketId);

            Assert.AreEqual(ticket.Name, TicketName);
        }

        public void FindTicketById_TicketIdNotFound_ThrowsException()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => TicketBL.FindTicketById(99));
        }
    }
}