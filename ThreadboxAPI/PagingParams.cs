namespace ThreadboxAPI
{
    public class PagingParams
    {
        const int maxPageSize = 50;

        public int CurrentPage { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }

        private int pageSize = 10;

        public PagingParams(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }   
    }
}
