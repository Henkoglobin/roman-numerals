namespace RomanNumerals {
    /// <summary>
    /// Converts an integer into the Roman Numeral representation
    /// </summary>
    public interface INumeralConverter {
        /// <summary>
        /// Converts an integer into the Roman Numeral representation
        /// </summary>
        /// <param name="value">The integer to convert. Must be in the range (0, 3000]</param>
        /// <returns>The roman numeral representation of the passed value.</returns>
        string ToNumeral(int value);
    }
}