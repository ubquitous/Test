using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossDoubleN.Models
{
    public class Crossword
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<znach> IdZnach { get; set; }
        public void zn(char[][] a)
        {
            IdZnach = new List<znach>();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                IdZnach.Add(new znach());
                IdZnach[i].number = i;
                IdZnach[i].IdCross = this;
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    IdZnach[i].row += a[i][j];
                }
            }
        }

        public void sort()
        {
            for (int i = 0; i < IdZnach.Count; i++)
            {
                if (IdZnach[i].number != i)
                {
                    znach per = IdZnach[i];
                    for (int j = i+1; j < IdZnach.Count; j++)
                    {
                        if (IdZnach[j].number == i)
                        {
                            znach p = IdZnach[i];
                            IdZnach[i] = IdZnach[j];
                            IdZnach[j] = p;
                        }
                    }
                }
            }
        }
    }

    public class znach
    {
        public int id { get; set; }
        public string row { get; set; }
        public int number { get; set; }
        public Crossword IdCross { get; set; }
    }
}