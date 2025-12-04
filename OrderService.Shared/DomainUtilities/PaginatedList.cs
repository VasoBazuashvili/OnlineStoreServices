using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Shared.DomainUtilities
{
	public class PaginatedList<T>
	{
		public IList<T> Data { get; private set; } = new List<T>();
		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }
		public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

		public PaginatedList() { }

		public PaginatedList(IList<T> data, int pageNumber, int pageSize, int totalCount)
		{
			Data = data ?? new List<T>();
			PageNumber = pageNumber > 0 ? pageNumber : 1;
			PageSize = pageSize > 0 ? pageSize : 10;
			TotalCount = totalCount >= 0 ? totalCount : data?.Count ?? 0;
		}
	}
}
