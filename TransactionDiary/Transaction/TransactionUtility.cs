public enum BalanceType
{
    Total,
    Income,
    Cost
}

// TODO 
// Break out helpers from TransactionService methods and place them here 
public static class TransactionUtility
{
    public static List<List<T>> SplitIntoPages<T>(List<T> items, int itemsOnPage)
    {
        var pages = new List<List<T>>();
        var nItems = 0;

        var curPage = new List<T>();

        foreach (var item in items)
        {
            curPage.Add(item);
            nItems += 1;

            if(nItems == 30)
            {
                pages.Add(curPage); 
                curPage = [];
            }    
        }

        pages.Add(curPage); 
        return pages;
    }
}