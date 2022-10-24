using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_2021_2022___Final_Project_2.Data;
using SD_340_W22SD_2021_2022___Final_Project_2.Models;

namespace SD_340_W22SD_2021_2022___Final_Project_2.DAL
{

    public class CommentRepository : IRepository<Comment> {

        private readonly ApplicationDbContext _context;
    
        public CommentRepository(ApplicationDbContext context) {
            _context = context;
        }

        public void Add(Comment entity) {
            _context.Add(entity);
        }

        public Comment Get(int id) {
            var comment = _context
                .Comment
                .Single(p => p.Id == id);
            return comment;
        }

        public Comment Get(Func<Comment, bool> predicate) {
            var comment = _context
                .Comment.Single(predicate);
            return comment;
        }

        public ICollection<Comment> GetAll() {
            var comments = _context
                .Comment.ToList();
            return comments;
        }

        public ICollection<Comment> GetList(Func<Comment, bool> predicate) {
            var comments = _context
                .Comment
                .Where(predicate)
                .ToList();
            return comments;
        }
    
        public Comment Update(Comment entity) {
            _context.Update(entity);
            var comment = _context
                .Comment
                .Single(p => p.Id == entity.Id);
            return comment;
        }

        public void Delete(Comment entity) {
            _context.Remove(entity);
        }

        public void Save() {
            _context.SaveChanges();
        }
    }
}