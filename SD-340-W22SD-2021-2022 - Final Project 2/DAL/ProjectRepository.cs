using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_2021_2022___Final_Project_2.Data;
using SD_340_W22SD_2021_2022___Final_Project_2.Models;

namespace SD_340_W22SD_2021_2022___Final_Project_2.DAL
{

    public class ProjectRepository : IRepository<Project> {

    private readonly ApplicationDbContext _context;
    
    public ProjectRepository(ApplicationDbContext context) {
        _context = context;
    }

    public void Add(Project entity) {
        _context.Add(entity);
    }

    public Project Get(int id) {
        var project = _context
            .Project
            .Single(p => p.Id == id);
        return project;
    }

    public Project Get(Func<Project, bool> predicate) {
        var project = _context
            .Project.Single(predicate);
        return project;
    }

    public ICollection<Project> GetAll() {
        var projects = _context
            .Project.ToList();
        return projects;
    }

    public ICollection<Project> GetList(Func<Project, bool> predicate) {
        var projects = _context
            .Project
            .Where(predicate)
            .ToList();
        return projects;
    }
    
    public Project Update(Project entity) {
        _context.Update(entity);
        var project = _context
            .Project
            .Single(p => p.Id == entity.Id);
        return project;
    }

    public void Delete(Project entity) {
        _context.Remove(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }
}
}