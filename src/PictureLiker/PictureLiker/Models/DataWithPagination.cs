using System;

namespace PictureLiker.Models
{
    public class DataWithPagination<T>
    {
        public T[] ItemsAtCurrentPage { get; set; }
        public int TotalPages => (int) Math.Ceiling((double) TotalItems / RecordsPerPage);
        public int RecordsPerPage { get; set; }
        public int Page { get; set; }
        public long TotalItems { get; set; }
    }
}