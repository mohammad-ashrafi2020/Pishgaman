﻿namespace Application.Utils;

public class BaseFilter
{
    public long EntityCount { get;  set; }
    public int CurrentPage { get;  set; }
    public int PageCount { get;  set; }
    public int StartPage { get;  set; }
    public int EndPage { get;  set; }
    public int Take { get;  set; }
    public void GeneratePaging(IQueryable<Object> data, int take, int currentPage)
    {
        var entityCount = data.Count();
        var pageCount = (int)Math.Ceiling(entityCount / (double)take);
        PageCount = pageCount;
        CurrentPage = currentPage;
        EndPage = (currentPage + 5 > pageCount) ? pageCount : currentPage + 5;
        EntityCount = entityCount;
        Take = take;
        StartPage = (currentPage - 4 <= 0) ? 1 : currentPage - 4;
    }
    public void GeneratePaging(long entityCount, int take, int currentPage)
    {
        var pageCount = (int)Math.Ceiling(entityCount / (double)take);
        PageCount = pageCount;
        CurrentPage = currentPage;
        EndPage = (currentPage + 5 > pageCount) ? pageCount : currentPage + 5;
        EntityCount = entityCount;
        Take = take;
        StartPage = (currentPage - 4 <= 0) ? 1 : currentPage - 4;
    }
    public void GeneratePaging(long entityCount, int pageCount, int take, int currentPage)
    {
        PageCount = pageCount;
        CurrentPage = currentPage;
        EndPage = (currentPage + 5 > pageCount) ? pageCount : currentPage + 5;
        EntityCount = entityCount;
        Take = take;
        StartPage = (currentPage - 4 <= 0) ? 1 : currentPage - 4;
    }
}

public class BaseFilterParam
{
    public int PageId { get; set; } = 1;
    public int Take { get; set; } = 10;
}

public class BaseFilter<TData> : BaseFilter
where TData : class
{
    public List<TData> Data { get; set; }
}