using OrbitalPdfTest.EntryTextAnalysis.Tokens;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines
{
    /// <summary>
    /// Rules to determine which columns the unstructured EntryText values belong in by placing into a fixed DataRow structure
    /// </summary>
    public class DefaultDataColumnRuleEngine : IDataEngineColumnRuleEngine
    {
        private const string WHITESPACE_PATTERN = "[ ]{2,}"; // matches any white space with length > 2, and treats as column separators
        private const string TEXT_PATTERN = @"\S+(?: \S+)*"; // matches any text that only has 1 white space character between each word

        public DataRow Run(string line, IWhiteSpaceSizeCalculator whiteSpaceCalculator)
        {
            // ASSUMPTION: First line always has 4 data elements
            // ASSUMPTION: The only value in the 4th column is on the first line of the schedule entry

            // pattern match all the whitespace and text portion of the line and convert to tokens
            MatchCollection textMatches = Regex.Matches(line, TEXT_PATTERN);
            MatchCollection whiteSpaceMatches = Regex.Matches(line, WHITESPACE_PATTERN);

            // simple validation rules for a line
            if (textMatches.Count > 4 || textMatches.Count == 0 || whiteSpaceMatches.Count > textMatches.Count) {
                return null;
            }

            DataRow dataRow = new DataRow();

            if (TrySetFourColumns(textMatches, dataRow))
            {
                return dataRow;
            }
            else if (TrySetThreeColumns(textMatches, dataRow))
            {
                return dataRow;
            }

            List<Token> tokens = new List<Token>();

            // using the regex matches set information about position and size in Tokens to help determine column allocation
            List<WhiteSpaceToken> whiteSpaceTokens = whiteSpaceMatches.Select(match => new WhiteSpaceToken(match, whiteSpaceCalculator)).ToList();
            List<TextToken> textTokens = textMatches.Select(match => new TextToken(match)).ToList();

            tokens.AddRange(whiteSpaceTokens);
            tokens.AddRange(textTokens);

            // order the text and whitespace tokens together by index ascending
            tokens = tokens.OrderBy(s => s.StartIndex).ToList();

            if (TrySetTwoColumns(tokens, textMatches, dataRow))
            {
                return dataRow;
            }
            else if (TrySetOneColumn(tokens, textMatches, whiteSpaceTokens.Count, dataRow))
            {
                return dataRow;
            }

            return dataRow;
        }

        private bool TrySetFourColumns(MatchCollection textMatches, DataRow dataRow)
        {
            if (textMatches.Count == 4)
            {
                dataRow.SetColumns(textMatches[0].Value, textMatches[1].Value, textMatches[2].Value, textMatches[3].Value);

                return true;
            }

            return false;
        }

        private bool TrySetThreeColumns(MatchCollection textMatches, DataRow dataRow)
        {
            if (textMatches.Count == 3)
            {
                dataRow.SetColumns(textMatches[0].Value, textMatches[1].Value, textMatches[2].Value);

                return true;
            }

            return false;
        }

        private bool TrySetTwoColumns(List<Token> tokens, MatchCollection textMatches, DataRow dataRow)
        {
            if (textMatches.Count == 2)
            {
                if (tokens.First() is WhiteSpaceToken)
                {
                    dataRow.SetColumns(null, textMatches[0].Value, textMatches[1].Value);

                    return true;
                }
                else if (tokens[1] is WhiteSpaceToken)
                {
                    if (((WhiteSpaceToken)tokens[1]).WhiteSpaceSize == WhiteSpaceSize.Small)
                    {
                        dataRow.SetColumns(textMatches[0].Value, textMatches[1].Value);
                    }
                    else
                    {
                        dataRow.SetColumns(textMatches[0].Value, null, textMatches[1].Value);
                    }

                    return true;
                }
            }

            return false;
        }

        private bool TrySetOneColumn(List<Token> tokens, MatchCollection textMatches, int whiteSpaceTokenCount, DataRow dataRow)
        {
            if (textMatches.Count == 1)
            {
                if (whiteSpaceTokenCount == 0)
                {
                    dataRow.SetColumns(textMatches[0].Value);

                    return true;
                }
                else if (whiteSpaceTokenCount == 1)
                {
                    if (((WhiteSpaceToken)tokens[0]).WhiteSpaceSize == WhiteSpaceSize.Small)
                    {
                        dataRow.SetColumns(null, textMatches[0].Value);
                    }
                    else
                    {
                        dataRow.SetColumns(null, null, textMatches[0].Value);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
