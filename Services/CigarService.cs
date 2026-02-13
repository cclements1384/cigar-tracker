using cigar_tracker.Models;

namespace cigar_tracker.Services;

public class CigarService
{
    private static List<Cigar> _cigars = new();
    private static int _nextId = 1;

    public List<Cigar> GetAllCigars()
    {
        return _cigars.OrderByDescending(c => c.DateSmoked).ToList();
    }

    public void AddCigar(Cigar cigar)
    {
        cigar.Id = _nextId++;
        _cigars.Add(cigar);
    }

    public void DeleteCigar(int id)
    {
        _cigars.RemoveAll(c => c.Id == id);
    }

    public Cigar? GetCigarById(int id)
    {
        return _cigars.FirstOrDefault(c => c.Id == id);
    }
}
