namespace BookStoreApp.API.Models;

public class QueryParameters
{
    private int _pageSize = 15;
    //which item will be starting to get next (initialy 0, next time 16, 31)
    public int StartIndex { get; set; }
    public int PageSize 
    { 
        get 
        {
            return _pageSize;
        } 
        set 
        {
            _pageSize = value; 
        } 
    }
}
