using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Shared.DomainUtilities
{
	public class PaginatedList<T>
	{
		public IEnumerable<T> Data { get; }
		public int PageNumber { get; }
		public int PageSize { get; }
		public int TotalCount { get; }
		public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

		public PaginatedList(IEnumerable<T> data, int totalCount, int pageNumber, int pageSize)
		{
			Data = data;
			TotalCount = totalCount;
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
	}
}
