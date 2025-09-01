namespace ContosoUniversity.Utils
{
    public class Sort
    {
        public enum Type
        {
            NAME_ASC = 0,
            NAME_DESC = 1,

            DATE_ASC = 2,
            DATE_DESC = 3
        }

        public static Type From(string stype)
        {
            switch (stype.ToLower())
            {
                case "name_desc":
                    return Type.NAME_DESC;
                case "date_desc":
                    return Type.DATE_DESC;
                case "date":
                case "date_asc":
                    return Type.DATE_ASC;
            }
            return Type.NAME_ASC;
        }
    }
}
