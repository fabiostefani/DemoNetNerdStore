namespace WebApp.MVC.Models;

public class PagedViewModel<T> where T : class
{
    public string ReferenceAction { get; set; } = string.Empty;
    public IEnumerable<T> List { get; set; } = new List<T>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string Query { get; set; } = string.Empty;
    public int TotalResults { get; set; }
    public double TotalPages => Math.Ceiling((double) TotalResults / PageSize);
}