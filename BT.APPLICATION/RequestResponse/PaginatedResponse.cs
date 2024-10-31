using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.APPLICATION.RequestResponse
{
    public class PaginatedResponse<T>
    {
        public T Data { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PaginatedResponse(T data, int pageNumber, int pageSize, int totalCount)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}