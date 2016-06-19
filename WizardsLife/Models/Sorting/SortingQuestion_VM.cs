using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardsLife.Models.Sorting
{
    public class SortingQuestion_VM
    {
        public Lib.Entity.SortingQuizQuestion Question { get; set; }

        public int? Value { get; set; }
    }
}