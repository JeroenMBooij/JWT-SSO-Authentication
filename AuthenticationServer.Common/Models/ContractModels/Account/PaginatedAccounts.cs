using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public class PaginatedAccounts
    {
        public long TotalItems { get; set; }
        public PageRange Range { get; set; }
        public long TotalPages { get; set; }
        public long CurrentPage { get; set; }
        public List<AccountWithId> Items { get; set; }
    }
}
