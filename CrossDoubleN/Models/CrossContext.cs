using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CrossDoubleN.Models
{
    public class CrossContext:DbContext
    {
        public CrossContext()
            : base("Crossword")
        { }
        public DbSet<Crossword> cross { get; set; }
        public DbSet<znach> znach { get; set; }
    }
}