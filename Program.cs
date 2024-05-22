namespace prototype_pattern
{
    //A demonstration of the Prototype pattern in C#

    //This is an example of how Excel would use the copy/paste functionality for cells with meta data of different types of cells
    public class Program
    {
        static void Main(string[] args)
        {
            List<ICell> cells = new List<ICell>();

            cells.Add(CellFactory.Create("Hello world"));
            cells.Add(CellFactory.Create("1234"));
            cells.Add(CellFactory.Create("01/10/2021"));

            //The actual cloning happening here - see that the format get copied from the new object
            cells.Add(cells[2].Clone());

            ((DateCell)cells[3]).Format = "MM/dd/yyyy";

            cells.Add(cells[3].Clone());

            foreach (var cell in cells)
            {
                Console.WriteLine($"{cell.Render()} || TYPE: {cell.GetType().Name}");
            }

            /* OUTPUT
             * 
             * Hello world || TYPE: TextCell
             * 1234 || TYPE: NumberCell
             * 2021-01-10 12:00:00 AM || FORMAT: dd/MM/yyyy || TYPE: DateCell
             * 2021-01-10 12:00:00 AM || FORMAT: MM/dd/yyyy || TYPE: DateCell
             * 2021-01-10 12:00:00 AM || FORMAT: MM/dd/yyyy || TYPE: DateCell
             * 
             */
        }
    }
    
    //The factory class here determines what type the cell is based on what is supplied (Date, Number, Text)
    public class CellFactory
    {
        public static ICell Create(string content)
        {
            if (DateTime.TryParse(content, out var date))
            {
                return new DateCell { Date = date };
            }
            else if (int.TryParse(content, out var number))
            {
                return new NumberCell { Number = number };
            }
            return new TextCell { Text = content };
        }
    }

    public interface ICell
    {
        //Used only to show the output of the cell
        public string Render();
        //The clone method all classes must implement to make the pattern work
        public ICell Clone();
    }

    public class TextCell : ICell
    {
        public required string Text { get; set; }

        public string Render()
        {
            return Text;
        }

        public ICell Clone()
        {
            return new TextCell { Text = Text };
        }
    }

    public class DateCell : ICell
    {
        public required DateTime Date { get; set;}
        public string Format { get; set; } = "dd/MM/yyyy";

        public string Render()
        {
            return Date.ToString() + " || FORMAT: " + Format;
        }

        public ICell Clone()
        {
            return new DateCell { Date = Date, Format = Format };
        }
    }

    public class NumberCell : ICell
    {
        public required int Number { get; set; }

        public string Render()
        {
            return Number.ToString();
        }

        public ICell Clone()
        {
            return new NumberCell { Number = Number };
        }
    }

}
