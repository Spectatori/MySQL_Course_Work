namespace Course_Work.Infrastructure;

public class CustomCommands
{
    public static string[] SplitOnCharAndNum(string input, char separator, int count)
    {
        string[] splitInput = new string[count];
        int counter = 0;
        string tempString = string.Empty;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == separator && counter != count - 1)
            {
                splitInput[counter] = tempString;
                counter++;
                tempString = string.Empty;
                continue;
            }
            tempString += input[i];
        }
        splitInput[counter] = tempString;

        return splitInput;
    }
    public static string[] SplitOnArrAndNum(string input, char[] separators, int count)
    {
        string[] splitInput = new string[count];
        int counter = 0;
        string tempString = string.Empty;

        for (int i = 0; i < input.Length; i++)
        {
            if (separators.Contains(input[i]) && counter != count - 1)
            {
                splitInput[counter] = tempString;
                counter++;
                tempString = string.Empty;
                continue;
            }
            tempString += input[i];
        }
        splitInput[counter] = tempString;

        return splitInput;

    }

    public static string[] SplitOnChar(string input, char separator)
    {
        int count = input.Count(c => c == separator) + 1;
        string[] splitInput = new string[count];
        int counter = 0;
        string tempString = string.Empty;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == separator)
            {
                splitInput[counter] = tempString;
                counter++;
                tempString = string.Empty;
                continue;
            }
            tempString += input[i];
        }
        splitInput[counter] = tempString;

        return splitInput;
    }
    public static string[] SplitOnArr(string input, char[] separators)
    {
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (separators.Contains(input[i]))
            {
                count++;
            }
        }
        count++;
        string[] splitInput = new string[count];
        int counter = 0;
        string tempString = string.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            if (separators.Contains(input[i]))
            {
                splitInput[counter] = tempString;
                counter++;
                tempString = string.Empty;
                continue;
            }
            tempString += input[i];
        }
        splitInput[counter] = tempString;

        return splitInput;

    }
    
    public static string Replace(string original, string search, string replacement)
    {
        // Initialize variables
        string modified = "";
        int i = 0;
        while (i < original.Length)
        {
            // Check if substring starting at i matches search string
            bool match = true;
            for (int j = 0; j < search.Length; j++)
            {
                if (i + j >= original.Length || original[i + j] != search[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                // If match, add replacement string to modified
                modified += replacement;
                i += search.Length;
            }
            else
            {
                // If not match, add current character to modified
                modified += original[i];
                i++;
            }
        }
        return modified;
    }
}