namespace MoviesApp.Validations
{
    public static class ValidationUtilities
    {
        public static string NonEmptyMessage = "The field {PropertyName} is required";
        public static string MaximumLengthMessage = "The field {PropertyName} should be less than {MaxLength} characters";
        public static string FirstLetterIsUpperCaseMessage = "The field {PropertyName} should start with uppercase";
        public static string EmailAddressMessage = "The field {PropertyName} is not a valid email address";

        public static string GreaterThanDate(DateTime value) => "The field {PropertyName} should be greater than " + value.ToString("yyyy-MM-dd");

        //CUSTOM VALIDATION
        public static bool FirstLetterUpperCase(string name)
        {
            if (string.IsNullOrEmpty(name))
                return true;

            return char.IsUpper(name[0]);
        }
    }
}
