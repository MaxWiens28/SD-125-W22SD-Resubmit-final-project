using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_2021_2022___Final_Project_2.Data;
using SD_340_W22SD_2021_2022___Final_Project_2.Models;

namespace SD_340_W22SD_2021_2022___Final_Project_2.DAL
{

    public class TicketRepository : IRepository<Ticket> {

        private readonly ApplicationDbContext _context;
    
        public TicketRepository(ApplicationDbContext context) {
            _context = context;
        }

        public void Add(Ticket entity) {
            _context.Add(entity);
        }

        public Ticket Get(int id) {
            var ticket = _context
                .Ticket
                .Single(p => p.Id == id);
            return ticket;
        }

        public Ticket Get(Func<Ticket, bool> predicate) {
            var ticket = _context
                .Ticket.Single(predicate);
            return ticket;
        }

        public ICollection<Ticket> GetAll() {
            var tickets = _context
                .Ticket.ToList();
            return tickets;
        }

        public ICollection<Ticket> GetList(Func<Ticket, bool> predicate) {
            var tickets = _context
                .Ticket
                .Where(predicate)
                .ToList();
            return tickets;
        }
    
        public Ticket Update(Ticket entity) {
            _context.Update(entity);
            var ticket = _context
                .Ticket
                .Single(p => p.Id == entity.Id);
            return ticket;
        }

        public void Delete(Ticket entity) {
            _context.Remove(entity);
        }

        public void Save() {
            _context.SaveChanges();
        }
    }
}
