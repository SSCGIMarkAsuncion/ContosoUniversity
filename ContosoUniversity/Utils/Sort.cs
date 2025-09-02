namespace ContosoUniversity.Utils
{
    public class Sort
    {
        public enum Type
        {
            NAME_ASC = 0,
            NAME_DESC = 1,

            DATE_ASC = 2,
            DATE_DESC = 3,

            ID_ASC = 4,
            ID_DESC = 5,

            CREDIT_ASC = 6,
            CREDIT_DESC = 7,

            DEPARTMENT_ASC = 8,
            DEPARTMENT_DESC = 9
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
                case "id_desc":
                    return Type.ID_DESC;
                case "id":
                case "id_asc":
                    return Type.ID_ASC;
                case "credit_desc":
                    return Type.CREDIT_DESC;
                case "credit":
                case "credit_asc":
                    return Type.CREDIT_ASC;
                case "department_desc":
                    return Type.DEPARTMENT_DESC;
                case "department":
                case "department_asc":
                    return Type.DEPARTMENT_ASC;
            }
            return Type.NAME_ASC;
        }
    }
}
